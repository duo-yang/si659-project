using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

public class BlockBehavior : MonoBehaviour {

  public static float lowerBound = -3F;

  public ColorBehavior colorManager;
  public bool isPicker = false;

  private GameObject _innerCube;
  private Light _pointLight;
  private Renderer _rend;
  private Transform _trans;
  private Vector3 _origin = new Vector3(-0.2F, -0.15F, 0.35F);

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
      if (!isPicker) {
        Destroy(gameObject);
      }
    }
    if (isPicker && Vector3.Distance(_trans.localPosition, _origin) > 0.5) {
      attach();
    }
	}

  public void setColor(Color color) {
    if (_notNull) {
      _rend.material.SetColor("_Color", color);
      _rend.material.SetColor("_EmissionColor", color);
      _pointLight.color = color;
    }
  }

  public void attach() {
    GetComponent<InteractionBehaviour>().ReleaseFromGrasp();
    GetComponent<AnchorableBehaviour>().TryAttach(true);
  }
}
