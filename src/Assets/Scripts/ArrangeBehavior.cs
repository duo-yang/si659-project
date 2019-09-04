using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeBehavior : MonoBehaviour {
  
  // private Renderer _rend;
  // private int _mode = 0;
  // private int _numMode = 2;

  public GameObject blockRoot;

  public bool snapOn = false;

  // Use this for initialization
  void Start () {
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void arrangeBlocks () {
    snapOn = ColorBehavior.colorManagerInstance.modeController.currentMode == 0;
    if (blockRoot != null) {
      if (snapOn) {
        for (int i = 0; i < blockRoot.transform.childCount; i++) {
          Transform trans = blockRoot.transform.GetChild(i);
          trans.Find("Cube").GetComponent<BoxCollider>().enabled = false;
        }
      } else {
        for (int i = 0; i < blockRoot.transform.childCount; i++) {
          Transform trans = blockRoot.transform.GetChild(i);
          trans.Find("Cube").GetComponent<BoxCollider>().enabled = true;
          trans.GetComponent<InteractionGlow>().useDefault();
        }
      }
      
    }
  }

}
