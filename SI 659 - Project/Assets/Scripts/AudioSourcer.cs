using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourcer : MonoBehaviour {

  public AudioManager audioManager;

  public AudioSource audioSource;

  private void Awake() {
  }

  // Use this for initialization
  void Start () {
    audioSource = GetComponent<AudioSource>();
    audioManager = ColorBehavior.colorManagerInstance.audioManager;
    if (audioManager != null) audioSource.clip = audioManager.getRandomClip();
    audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
