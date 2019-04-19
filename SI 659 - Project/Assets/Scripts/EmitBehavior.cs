using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class EmitBehavior : MonoBehaviour {

  public InteractionSlider Slider;

  public float legendMin = 0F;
  public float legendMax = 0.8F;
  public float defaultLegend = 0.2F;
  public bool useSliderDefault = false;

  public ColorBehavior colorManager;

  public TextMesh ValueText;

  public enum ValueChannel { Light, Hue };
  public ValueChannel thisChannel = ValueChannel.Light;

  private Color _legendColor = Color.black;
  private Renderer _rend;
  private bool _reverse = false;

  // Use this for initialization
  void Start () {
    colorManager = ColorBehavior.colorManagerInstance;
    _rend = GetComponent<Renderer>();
    if (Slider.maxHorizontalValue < Slider.minHorizontalValue) {
      _reverse = true;
      float temp = legendMin;
      legendMin = legendMax;
      legendMax = temp;
    }
    if (useSliderDefault) defaultLegend = mapping(Slider.defaultHorizontalValue);
    setLegendColor(defaultLegend, thisChannel);
    if (ValueText != null) ValueText.text = Slider.defaultHorizontalValue.ToString();
  }
	
	float mapping(float value) {
    if (!_reverse && (value < Slider.minHorizontalValue || value > Slider.maxHorizontalValue)) return defaultLegend;
    if (_reverse && (value > Slider.minHorizontalValue || value < Slider.maxHorizontalValue)) return defaultLegend;
    return (value - Slider.minHorizontalValue) / (Slider.maxHorizontalValue - Slider.minHorizontalValue) * (legendMax - legendMin) + legendMin;
  }

  void setLegendColor(float legendValue, ValueChannel channel) {
    if (channel == ValueChannel.Light) {
      _legendColor = new Color(legendValue, legendValue, legendValue);
      _rend.material.SetColor("_EmissionColor", _legendColor);
    } else if (channel == ValueChannel.Hue) {
      if (colorManager != null) _legendColor = Color.HSVToRGB(legendValue, colorManager.currentSaturation, colorManager.currentValue);
      Debug.Log(Slider.minHorizontalValue + ", " + Slider.maxHorizontalValue + ", " + legendMin + ", " + legendMax);
      _rend.material.SetColor("_EmissionColor", _legendColor);
    }
  }

  public void updateButtonLight(float intensity) {
    if (thisChannel != ValueChannel.Light) return;
    setLegendColor(mapping(intensity), ValueChannel.Light);
    if (ValueText != null) ValueText.text = intensity.ToString("F");
  }

  public void updateButtonColor(float hue) {
    if (thisChannel != ValueChannel.Hue) return;
    setLegendColor(mapping(hue), ValueChannel.Hue);
    if (ValueText != null) ValueText.text = hue.ToString("F");
  }
}
