using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapBehavior : MonoBehaviour {

  public GameObject blockRoot;
  public GameObject rotationRoot;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void arrangeBlocks() {
    if (blockRoot != null && rotationRoot != null) {
      Quaternion rot = rotationRoot.transform.rotation;
      for (int i = 0; i < blockRoot.transform.childCount; i++) {
        Transform trans = blockRoot.transform.GetChild(i);
        trans.rotation = rot;
      }
    }
  }
}
