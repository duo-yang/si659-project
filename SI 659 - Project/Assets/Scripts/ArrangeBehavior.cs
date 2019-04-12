using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeBehavior : MonoBehaviour {
  
  // private Renderer _rend;
  // private int _mode = 0;
  // private int _numMode = 2;

  public GameObject blockRoot;
  public GameObject rotationRoot;

  // Use this for initialization
  void Start () { 

  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void arrangeBlocks () {
    if (blockRoot != null && rotationRoot != null) {
      Quaternion rot = rotationRoot.transform.rotation;
      for (int i = 0; i < blockRoot.transform.childCount; i++) {
        blockRoot.transform.GetChild(i).transform.rotation = rot;
      }
    }
  }

}
