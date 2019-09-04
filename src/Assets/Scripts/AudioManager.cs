using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

  public AudioClip[] audioClips;
  public int numClips = 0;
  private int _last = 6;

  private System.Random _random = new System.Random();

  private void Awake() {
    if (audioClips != null) numClips = audioClips.Length;
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public AudioClip getRandomClip() {
    if (audioClips == null) return null;

    int temp = _random.Next(numClips);
    while (temp == _last) { temp = _random.Next(numClips); }
    _last = temp;
    return audioClips[_last];
  }
}
