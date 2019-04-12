using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour {

  public static float lowerBound = -1.5F;

  public ColorBehavior colorManager;
  public bool isPicker = false;

  private GameObject _innerCube;
  private Light _pointLight;
  private Renderer _rend;
  private Transform _trans;

  private bool _notNull = false;

  void Awake() {
    colorManager = ColorBehavior.colorManagerInstance;
  }

  // Use this for initialization
  void Start () {
    _innerCube = this.transform.Find("Cube").gameObject;
    _pointLight = this.transform.Find("Point Light").gameObject.GetComponent<Light>();
    _trans = this.transform;
    if (_innerCube != null && _pointLight != null) {
      _rend = _innerCube.GetComponent<Renderer>();
      _notNull = true;
      setColor(colorManager.getColor());
    }
  }
	
	// Update is called once per frame
	void Update () {
    if (PhysicsBehavior.gravityOn() && _trans.position.y < lowerBound) {
      if (!isPicker) Destroy(gameObject);
    } 
	}

  public void setColor(Color color) {
    if (_notNull) {
      _rend.material.SetColor("_Color", color);
      _rend.material.SetColor("_EmissionColor", color);
      _pointLight.color = color;
    }
  }
}
