using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBehavior : MonoBehaviour {

  public MonoBehaviour[] modeControllers;
  public Color[] modeColors;
  public string[] modeNamesText;
  public string[] modeInfos;

  public TextMesh buttonText;
  public TextMesh infoText;
  public int currentMode = 1;

  private Renderer _rend;
  private int _numMode = 0;

  void OnValidate() {
    if (modeControllers.Length != modeNamesText.Length) {
      Debug.LogWarning("Number of mode controllers does not match number of their names.");
    }
  }

  // Use this for initialization
  void Start () {
    _rend = GetComponent<Renderer>();
    _rend.material.SetColor("_EmissionColor", Color.black);
    if (modeControllers.Length == modeNamesText.Length) {
      _numMode = modeControllers.Length;
    }
    useMode(currentMode);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void useMode(int mode) {
    // Enable corresponding mode and disable others
    for (int i = 0; i < _numMode; i++) {
      modeControllers[i].enabled = (i == mode) ? true : false;
    }
    // Scale mode
    if (mode == 0) {
      ColorBehavior.colorManagerInstance.facingCamera.OnEndFacingCamera.Invoke();
      // ColorBehavior.colorManagerInstance.colorPicker.enabled = false;
      // ColorBehavior.colorManagerInstance.colorAnchors.SetActive(false);
    }
    // Create mode
    if (mode == 1) {
      ColorBehavior.colorManagerInstance.facingCamera.OnBeginFacingCamera.Invoke();
      // ColorBehavior.colorManagerInstance.colorPicker.enabled = true;
      // ColorBehavior.colorManagerInstance.colorAnchors.SetActive(true);
    }

    // Set button color
    _rend.material.SetColor("_EmissionColor", (modeColors.Length > mode) ? modeColors[mode] : Color.black);
    // Set button text
    if (buttonText != null) buttonText.text = modeNamesText[mode];
    if (infoText != null) infoText.text = (modeInfos.Length > mode) ? modeInfos[mode] : "";
  }

  public void toggleMode () {
    if (currentMode < _numMode - 1) {
      currentMode++;
    } else {
      currentMode = 0;
    }
    useMode(currentMode);
  }

}
