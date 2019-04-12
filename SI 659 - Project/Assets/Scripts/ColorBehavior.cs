using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBehavior : MonoBehaviour {

  public static ColorBehavior colorManagerInstance;

  public BlockBehavior colorPicker;

  public Color[] palette;
  public int currentColorID = 0;

  private int _numColor = 0;

  void Awake() {
    colorManagerInstance = this;
  }

  // Use this for initialization
  void Start () {
    if (palette != null) _numColor = palette.Length;
    if (colorPicker != null) colorPicker.setColor(getColor());
    currentColorID = 0;
  }
	
	// Update is called once per frame
	void Update () {

	}

  public Color getColor(int colorID) {
    return (colorID < _numColor) ? palette[colorID] : Color.white;
  }
  public Color getColor() {
    return getColor(currentColorID);
  }

  public void updateColorID(int colorID) {
    if (colorID >= 0 && colorID < _numColor) currentColorID = colorID;
    if (colorPicker != null) colorPicker.setColor(getColor());
  }
}
