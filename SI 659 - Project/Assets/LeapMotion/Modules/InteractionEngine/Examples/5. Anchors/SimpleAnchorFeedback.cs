/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Examples {

  [RequireComponent(typeof(Anchor))]
  [AddComponentMenu("")]
  public class SimpleAnchorFeedback : MonoBehaviour {

    public Transform scaleTarget;
    public GameObject materialTarget;

    public bool feedbackMaterial = false;

    private Anchor _anchor;

    private Vector3 _initScaleVector;
    private float _curScale = 1F;

    private Renderer _rend;
    private Color _curColor;
    private Color _targetColor;

    void Start() {
      _anchor = GetComponent<Anchor>();

      _initScaleVector = scaleTarget.transform.localScale;
      _rend = materialTarget.GetComponent<Renderer>();
      _targetColor = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, 0.6F);
      _curColor = new Color(_rend.material.color.r, _rend.material.color.g, _rend.material.color.b, 0.6F);
    }

    void Update() {
      float _targetScale = 1F;
      _targetColor.a = 0.6F;

      if (_anchor.isPreferred) {
        _targetScale = 1.3F;
        _targetColor.a = 1F;
      }

      if (_anchor.hasAnchoredObjects) {
        _targetScale = 2.4F;
        _targetColor.a = 0F;
      }

      _curScale = Mathf.Lerp(_curScale, _targetScale, 20F * Time.deltaTime);
      _curColor.a = Mathf.Lerp(_curColor.a, _targetColor.a, 20F * Time.deltaTime);

      scaleTarget.transform.localScale = _curScale * _initScaleVector;
      _rend.material.color = _curColor;
    }

  }

}
