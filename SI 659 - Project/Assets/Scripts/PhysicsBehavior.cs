using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBehavior : MonoBehaviour {

  public float gravity_Y = -4.905F;
  // public GameObject gravityButton;
  public Color emitColor = Color.black;

  private Vector3 _gravity;
  private bool _gravityOn = false;
  private Renderer _rend;

  // Use this for initialization
  void Start () {
    _rend = GetComponent<Renderer>();
    _rend.material.SetColor("_EmissionColor", Color.black);
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
      _rend.material.SetColor("_EmissionColor", _gravityOn ? emitColor : Color.black);
    }
  }

}
