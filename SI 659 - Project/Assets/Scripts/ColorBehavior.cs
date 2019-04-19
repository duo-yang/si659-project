using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Examples;
using Leap.Unity.Animation;
using UnityEngine;
using Leap.Unity;

public class ColorBehavior : MonoBehaviour {

  public static ColorBehavior colorManagerInstance;

  public BlockBehavior colorPicker;
  public OnboardControl onboards;
  public ModeBehavior modeController;
  public SimpleFacingCameraCallbacks facingCamera;
  public PinchDetector PinchDetectorA;
  public PinchDetector PinchDetectorB;

  public GameObject colorAnchors;

  public Color[] palette;
  public int currentColorID = 1;
  public Color currentColor = Color.white;
  public float currentHue = 0f;
  public float currentSaturation = 0f;
  public float currentValue = 0f;

  private int _numColor = 0;

  public TransformTweenBehaviour magentaAnchor;
  public TransformTweenBehaviour redAnchor;
  public TransformTweenBehaviour blueAnchor;

  void Awake() {
    colorManagerInstance = this;
  }

  // Use this for initialization
  void Start () {
    if (palette != null) {
      _numColor = palette.Length;
      Color.RGBToHSV(palette[currentColorID], out currentHue, out currentSaturation, out currentValue);
      currentColor = palette[currentColorID];
    }
    if (colorPicker != null) colorPicker.setColor(getColor());
  }
	
	// Update is called once per frame
	void Update () {

	}

  public Color getColor(int colorID) {
    return (colorID < _numColor) ? palette[colorID] : Color.white;
  }
  public Color getColor() {
    return currentColor;
  }

  public void updateColorID(int colorID) {
    if (colorID >= 0 && colorID < _numColor) {
      currentColorID = colorID;
      currentColor = palette[colorID];
      Color.RGBToHSV(palette[colorID], out currentHue, out currentSaturation, out currentValue);
    }
    updateColor();
  }

  public void updateColor(float hue) {
    currentHue = hue;
    updateColor();
  }
  public void updateColor() {
    currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);
    if (colorPicker != null) colorPicker.setColor(getColor());
  }

  public void showColorAnchors() {
    // Only show on create mode
    if (modeController.currentMode == 1) {
      magentaAnchor.PlayForwardAfterDelay(0F);
      redAnchor.PlayForwardAfterDelay(0.05F);
      blueAnchor.PlayForwardAfterDelay(0.1F);
    }
  }
}
