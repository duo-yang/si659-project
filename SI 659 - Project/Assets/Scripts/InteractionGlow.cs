/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using Leap.Unity;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This simple script changes the color of an InteractionBehaviour as
/// a function of its distance to the palm of the closest hand that is
/// hovering nearby.
/// </summary>
public class InteractionGlow : MonoBehaviour {

  private ColorBehavior colorManager;

  [Header("InteractionBehaviour Colors")]
  public Color defaultColor;
  public float domainMin = 0f;
  public float domainMax = 0.5f;
  public float rangeMin = 1f;
  public float rangeMax = 0f;

  public Renderer _rend;
  public Light _pointLight;

  void Start() {

    _rend = this.transform.Find("Cube").GetComponent<Renderer>();
    _pointLight = this.transform.Find("Point Light").GetComponent<Light>();

    colorManager = ColorBehavior.colorManagerInstance;
    if (colorManager != null) {
      defaultColor = colorManager.getColor();
    }

  }

  void Update() {
    if (colorManager.arrangeManager.snapOn && _rend != null) {

      float alphaA = mapping(Vector3.Distance(this.transform.position, colorManager.PinchDetectorA.transform.position));
      float alphaB = mapping(Vector3.Distance(this.transform.position, colorManager.PinchDetectorB.transform.position));
      float alpha = (alphaA > alphaB) ? alphaA : alphaB;

      // Debug.Log(Vector3.Distance(this.transform.position, colorManager.PinchDetectorA.transform.position));

      _rend.material.SetColor("_Color", new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha));
      _pointLight.intensity = alpha;
    }
  }

  float mapping(float value) {
    if (value > domainMax) return rangeMax;
    if (value < domainMin) return rangeMin;
    return (value - domainMin) / (domainMax - domainMin) * (rangeMax - rangeMin) + rangeMin;
  }

  public void useDefault() {
    _rend.material.SetColor("_Color", defaultColor);
    _pointLight.intensity = rangeMin;
  }

}
