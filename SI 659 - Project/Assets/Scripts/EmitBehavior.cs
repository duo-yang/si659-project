using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitBehavior : MonoBehaviour {

  public float intensityMin = 0.5F;
  public float intensityMax = 2.5F;
  public float lightMin = 0F;
  public float lightMax = 0.8F;
  public float defaultIntensity = 1F;

  public TextMesh intensityText;

  private float _defaultLight = 0F;
  private Color _buttonColor = Color.black;
  private Renderer _rend;

  // Use this for initialization
  void Start () {
    _rend = GetComponent<Renderer>();
    _defaultLight = mapIntensity(defaultIntensity);
    setButtonColor(_defaultLight);
    if (intensityText != null) intensityText.text = defaultIntensity.ToString();
  }
	
	float mapIntensity(float intensity) {
    if (intensity < intensityMin || intensity > intensityMax) return _defaultLight;
    return (intensity - intensityMin) / (intensityMax - intensityMin) * (lightMax - lightMin) + lightMin;
  }

  void setButtonColor(float light) {
    _buttonColor.r = light;
    _buttonColor.g = light;
    _buttonColor.b = light;
  }

  public void updateButtonLight(float intensity) {
    setButtonColor(mapIntensity(intensity));
    _rend.material.SetColor("_EmissionColor", _buttonColor);
    if (intensityText != null) intensityText.text = intensity.ToString("F");
  }
}
