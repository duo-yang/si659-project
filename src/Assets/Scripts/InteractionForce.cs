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
[RequireComponent(typeof(InteractionBehaviour))]
public class InteractionForce : MonoBehaviour {

  [Tooltip("If enabled, the object will lerp to its hoverColor when a hand is nearby.")]
  public bool useHover = true;

  [Tooltip("If enabled, the object will use its primaryHoverColor when the primary hover of an InteractionHand.")]
  public bool usePrimaryHover = false;

  [Header("InteractionBehaviour Colors")]
  public Color defaultColor = Color.Lerp(Color.black, Color.white, 0.1F);
  public Color suspendedColor = Color.red;
  public Color hoverColor = Color.Lerp(Color.black, Color.white, 0.7F);
  public Color primaryHoverColor = Color.Lerp(Color.black, Color.white, 0.8F);

  [Header("InteractionButton Colors")]
  [Tooltip("This color only applies if the object is an InteractionButton or InteractionSlider.")]
  public Color pressedColor = Color.white;

  public float forceMultiplier = 0.15f;
  public float threshold = 0.3f;

  private Material _material;

  private InteractionBehaviour _intObj;
  private PinchDetector _pinchA;
  private PinchDetector _pinchB;
  private Vector3 forceA;
  private Vector3 forceB;
  private Vector3 forceO;

  void Start() {
    _intObj = GetComponent<InteractionBehaviour>();

    _pinchA = ColorBehavior.colorManagerInstance.PinchDetectorA;
    _pinchB = ColorBehavior.colorManagerInstance.PinchDetectorB;

    Renderer renderer = GetComponent<Renderer>();
    if (renderer == null) {
      renderer = GetComponentInChildren<Renderer>();
    }
    if (renderer != null) {
      _material = renderer.material;
    }
  }

  void FixedUpdate() {
    if (PhysicsBehavior._gravityOn) {
      Vector3 attractA = (_pinchA.transform.position - _intObj.transform.position);
      Vector3 attractB = (_pinchB.transform.position - _intObj.transform.position);
      Vector3 attractO = (Vector3.zero - _intObj.transform.position);
      if (attractB.magnitude < threshold) {
        forceB = (threshold - attractB.magnitude) * attractB * forceMultiplier * (_pinchB.IsPinching ? 1 : -1);
      } else {
        forceB = (attractB.magnitude - threshold) * attractB * forceMultiplier;
      }
      forceA = ((attractA.magnitude < threshold) ? threshold - attractA.magnitude : attractA.magnitude - threshold) * attractA * forceMultiplier * (_pinchA.IsPinching ? 1 : -1);
      if (attractO.magnitude > 3 * threshold) forceO = attractO * (attractO.magnitude - 3 * threshold);
      _intObj.AddLinearAcceleration(forceB + forceO);
    }
  }

}
