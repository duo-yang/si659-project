using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.DetectionExamples {

  public class PinchSpawn : MonoBehaviour
  {

    [Tooltip("Each pinch detector can draw one line at a time.")]
    [SerializeField]
    private PinchDetector[] _pinchDetectors;

    // [SerializeField]
    // private Material _material;

    [SerializeField]
    private GameObject[] _blockPrefab;
    public int chosenPrefab;

    public GameObject blockRoot;
    [SerializeField]
    private ClearBehavior _clearBehavior;

    private DrawState[] _drawStates;

    void Awake()
    {
      if (_pinchDetectors.Length == 0)
      {
        Debug.LogWarning("No pinch detectors were specified!  PinchSpawn can not draw any lines without PinchDetectors.");
      }
    }

    // Use this for initialization
    void Start()
    {
      _drawStates = new DrawState[_pinchDetectors.Length];
      for (int i = 0; i < _pinchDetectors.Length; i++)
      {
        _drawStates[i] = new DrawState(this, _blockPrefab, blockRoot);
      }
    }

    // Update is called once per frame
    void Update()
    {
      for (int i = 0; i < _pinchDetectors.Length; i++)
      {
        var detector = _pinchDetectors[i];
        var drawState = _drawStates[i];

        if (detector.DidStartHold)
        {
          drawState.BeginNewBlock(detector.Position);
          _clearBehavior.newBlock();
        }

        if (detector.DidRelease)
        {
          drawState.FinishBlock();
        }

        if (detector.IsHolding)
        {
          drawState.UpdateBlock(detector.Position);
        }
      }
    }

    private class DrawState
    {
      private PinchSpawn _parent;
      private GameObject _root;
      private GameObject[] _prefabs;
      private GameObject _blockObj;

      public DrawState(PinchSpawn parent, GameObject[] prefabs, GameObject root) {
        _parent = parent;
        _prefabs = prefabs;
        _root = root;
      }

      public GameObject BeginNewBlock(Vector3 position) {
        _blockObj = (GameObject) Instantiate(_prefabs[_parent.chosenPrefab], position, Quaternion.identity);
        _blockObj.transform.parent = _root.transform;
        // _blockObj.AddComponent<MeshRenderer>().sharedMaterial = _parent._material;

        return _blockObj;
      }

      public void UpdateBlock(Vector3 position) {
        _blockObj.transform.position = position;
      }

      public void FinishBlock() {
        _blockObj = null;
      }

    }

  }

}