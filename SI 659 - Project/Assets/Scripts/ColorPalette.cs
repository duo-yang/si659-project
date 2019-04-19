using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class ColorPalette : MonoBehaviour {

    public InteractionSlider Slider;

    public Color paletteColor = Color.black;
    private Renderer _rend;

    private float _hue;
    private float _sat;
    private float _val;

    // Use this for initialization
    void Start()
    {
        Color.RGBToHSV(paletteColor, out _hue, out _sat, out _val);
        _rend = GetComponent<Renderer>();
        _rend.material.SetColor("_EmissionColor", paletteColor);
    }

    public void setPaletteColor(Color newColor)
    {
        paletteColor = newColor;
        Color.RGBToHSV(paletteColor, out _hue, out _sat, out _val);
        _rend.material.SetColor("_EmissionColor", paletteColor);
    }

    public void usePaletteColor()
    {
        Slider.HorizontalSlideEvent.Invoke(_hue);
    }
}
