using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBehavior : MonoBehaviour {

  public float gravity_Y = -4.905F;
  // public GameObject gravityButton;
  public Color emitColor = Color.black;
  public Color defaultColor = new Color(0.3F, 0.3F, 0.3F);

  private Vector3 _gravity;
  private static bool _gravityOn = false;

  private Renderer _rend;

  // Use this for initialization
  void Start () {
    _rend = GetComponent<Renderer>();
    _rend.material.SetColor("_EmissionColor", defaultColor);
    _gravity = new Vector3(0, gravity_Y, 0);
    Physics.gravity = _gravityOn? _gravity : Vector3.zero;
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void toggleGravity () {
    _gravityOn = !_gravityOn;
    Physics.gravity = _gravityOn ? _gravity : Vector3.zero;
    if (_rend != null) {
      _rend.material.SetColor("_EmissionColor", _gravityOn ? emitColor : defaultColor);
    }
  }

  public static bool gravityOn() {
    return _gravityOn;
  }

}
