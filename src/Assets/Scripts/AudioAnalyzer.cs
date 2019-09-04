using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioAnalyzer : MonoBehaviour {
  public AudioClip[] audioClips;
  private int _curClip = 0;

  public static int _bandCounts = 8;
  public static int _sampleCounts = 512;
  private bool _playing = true;

  public TextMesh indicator;
  public TextMesh effect;

  public float[,] xMap = new float[2, 2];
  public float[,] yMap = new float[2, 2];
  public float[,] zMap = new float[2, 2];

  public bool useBuffer = true;
  public float bufferDecreaseSpeed = 0.005f;
  public float audioProfile = 5.0f;

  [HideInInspector]
  public static float[,] audioBand, audioBandBuffer;

  public static float[] Amplitude = new float[3];
  public static float[] AmplitudeBuffer = new float[3];

  public enum Channel { Left = 0, Right = 1, Stereo = 2 };

  private AudioSource _audioSource;
  public GameObject tracker;
  private Transform _tracker;

  private float[] samplesLeft, samplesRight;

  private float[,] freqBands;
  private float[,] bandBuffer;

  private float[,] _bufferDecrease;
  private float[,] _freqBandHighest;
  private float[] _ampHighest = new float[3];

  // Use this for initialization
  void Start () {

    // panStereo
    xMap[0, 0] = -0.4F;
    xMap[0, 1] = 0.4F;
    xMap[1, 0] = -1F;
    xMap[1, 1] = 1F;

    // pitch
    yMap[0, 0] = -0.8F;
    yMap[0, 1] = 0.4F;
    yMap[1, 0] = -1F;
    yMap[1, 1] = 2F;

    // volume
    zMap[0, 0] = -0.8F;
    zMap[0, 1] = 1.2F;
    zMap[1, 0] = 0F;
    zMap[1, 1] = 1F;

    _audioSource = GetComponent<AudioSource>();
    if (tracker != null) _tracker = tracker.transform;

    audioBand = new float[_bandCounts, 3];
    audioBandBuffer = new float[_bandCounts, 3];

    samplesLeft = new float[_sampleCounts];
    samplesRight = new float[_sampleCounts];

    freqBands = new float[_bandCounts, 3];
    bandBuffer = new float[_bandCounts, 3];

    _bufferDecrease = new float[_bandCounts, 3];

    _freqBandHighest = new float[_bandCounts, 3];

    useAudioProfile(audioProfile);
	}
	
	// Update is called once per frame
	void Update () {
    updateAudio();
    getSpectrumfromAudioSource();
    getFreqBands();
	}

  private float mapping(float value, float[,] map) {
    if (value > map[0, 1]) value = map[0, 1];
    if (value < map[0, 0]) value = map[0, 0];
    return (value - map[0, 0]) * (map[1, 1] - map[1, 0]) / (map[0, 1] - map[0, 0]) + map[1, 0];
  }

  private void updateAudio() {
    if (_tracker != null) {
      float xEffect = mapping(_tracker.position.x, xMap);
      float yEffect = mapping(_tracker.position.y, yMap);
      float zEffect = mapping(_tracker.position.z, zMap);

      _audioSource.pitch = yEffect;
      _audioSource.volume = zEffect;
      _audioSource.panStereo = xEffect;

      indicator.text = _tracker.position.x.ToString("F") + ", " + _tracker.position.y.ToString("F") + ", " + _tracker.position.z.ToString("F");
      effect.text = "Stereo: " + xEffect.ToString("F") + ", pitch:" + yEffect.ToString("F") + ", Volume:" + zEffect.ToString("F");
    }
  }

  private void getSpectrumfromAudioSource (AudioSource source) {
    if (source != null) {
      source.GetSpectrumData(samplesLeft, (int) Channel.Left, FFTWindow.Blackman);
      source.GetSpectrumData(samplesRight, (int) Channel.Right, FFTWindow.Blackman);
    }
  }

  private void getSpectrumfromAudioSource () {
    getSpectrumfromAudioSource(_audioSource);
  }

  private void getFreqBands () {
    int count = 0;
    float[] curAmp = new float[3];
    float[] curAmpBuffer = new float[3];

    for (int i = 0; i < _bandCounts; i++) {
      float avgLeft = 0;
      float avgRight = 0;
      int sampleCount = (int)Mathf.Pow(2, i + 1);
      if (i == _bandCounts - 1) {
        sampleCount += 2;
      }
      for (int j = 0; j < sampleCount; j++) {
        avgLeft += samplesLeft[count] * (count + 1);
        avgRight += samplesRight[count] * (count + 1);
        count++;
      }
      avgLeft /= count;
      avgRight /= count;
      freqBands[i, (int) Channel.Left] = avgLeft * 10;
      freqBands[i, (int) Channel.Right] = avgRight * 10;
      freqBands[i, (int) Channel.Stereo] = (freqBands[i, (int) Channel.Left] + freqBands[i, (int) Channel.Right]) / 2;

      for (int k = 0; k < 3; k++) {
        if (useBuffer) {
          if (bandBuffer[i, k] <= freqBands[i, k]) {
            _freqBandHighest[i, k] = (_freqBandHighest[i, k] < freqBands[i, k]) ? freqBands[i, k] : _freqBandHighest[i, k];
            bandBuffer[i, k] = freqBands[i, k];
            _bufferDecrease[i, k] = bufferDecreaseSpeed;
          } else {
            bandBuffer[i, k] -= _bufferDecrease[i, k];
            _bufferDecrease[i, k] *= 1.2f;
          }

          audioBandBuffer[i, k] = bandBuffer[i, k] / _freqBandHighest[i, k];
          curAmpBuffer[k] += audioBandBuffer[i, k];

        }

        audioBand[i, k] = freqBands[i, k] / _freqBandHighest[i, k];
        curAmp[k] += audioBand[i, k];

      }
    }

    for (int k = 0; k < 3; k++) {
      if (curAmp[k] > _ampHighest[k]) _ampHighest[k] = curAmp[k];

      Amplitude[k] = curAmp[k] / _ampHighest[k];
      AmplitudeBuffer[k] = curAmpBuffer[k] / _ampHighest[k];
    }
  }

  private void useAudioProfile(float profile) {
    for (int i = 0; i < _bandCounts; i++) {
      for (int k = 0; k < 3; k++) {
        _freqBandHighest[i, k] = profile;
      }
    }
  }

  public void togglePlay() {
    if (_playing) { _audioSource.Pause(); }
    else { _audioSource.UnPause(); }
    _playing = !_playing;
  }

  public void nextClip() {
    _audioSource.Stop();
    if (++_curClip >= audioClips.Length) _curClip = 0;
    _audioSource.clip = audioClips[_curClip];
    _audioSource.Play();
  }

  public void lastClip() {
    _audioSource.Stop();
    if (--_curClip < 0) _curClip = audioClips.Length - 1;
    _audioSource.clip = audioClips[_curClip];
    _audioSource.Play();
  }

}
