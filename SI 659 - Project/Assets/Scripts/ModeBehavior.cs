using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBehavior : MonoBehaviour {

  public MonoBehaviour[] modeControllers;
  public Color[] modeColors;
  public string[] modeNamesText;

  public TextMesh buttonText;
  
  private Renderer _rend;
  private int _mode = 0;
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
    useMode(_mode);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void useMode(int mode) {
    // Enable corresponding mode and disable others
    for (int i = 0; i < _numMode; i++) {
      modeControllers[i].enabled = (i == mode) ? true : false;
    }
    // Set button color
    _rend.material.SetColor("_EmissionColor", (modeColors.Length > mode) ? modeColors[mode] : Color.black);
    // Set button text
    if (buttonText != null) buttonText.text = modeNamesText[mode];

  }

  public void toggleMode () {
    if (_mode < _numMode - 1) {
      _mode++;
    } else {
      _mode = 0;
    }
    useMode(_mode);
  }

}
