using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.DetectionExamples {

  public class PinchSpawn : MonoBehaviour
  {

    [Tooltip("Each pinch detector can draw one line at a time.")]
    [SerializeField]
    private PinchDetector[] _pinchDetectors;

    public ColorBehavior colorManager;

    private PinchDetector _pinchDetectorA;
    private PinchDetector _pinchDetectorB;

    // [SerializeField]
    // private Material _material;

    [SerializeField]
    private GameObject[] _blockPrefab;
    public int chosenPrefab;

    public GameObject blockRoot;

    public float noSpawnRangeMultiplier = 1.5f;

    [SerializeField]
    private ClearAllBehavior _clearBehavior;

    private DrawState[] _drawStates;
    private bool[] _didSpawn;

    private GameObject[] movableVoxel;
    private float _distanceRef = 1.0f;
    private bool _scaling = false;
    private Vector3 _startScale = Vector3.one;

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
      colorManager = ColorBehavior.colorManagerInstance;

      _pinchDetectorA = colorManager.PinchDetectorA;
      _pinchDetectorB = colorManager.PinchDetectorB;

      _drawStates = new DrawState[_pinchDetectors.Length];
      _didSpawn = new bool[_pinchDetectors.Length];
      for (int i = 0; i < _pinchDetectors.Length; i++)
      {
        _drawStates[i] = new DrawState(this, _blockPrefab, blockRoot);
        _didSpawn[i] = false;
      }
      movableVoxel = new GameObject[_pinchDetectors.Length];
    }

    // Update is called once per frame
    void Update()
    {

      bool spawnable = false;

      for (int i = 0; i < _pinchDetectors.Length; i++) {
        var detector = _pinchDetectors[i];
        var drawState = _drawStates[i];

        if (detector.DidStartHold) {
          if (!colorManager.onboards.criteria[1])
          {
            colorManager.onboards.updateCriteria(1);
            colorManager.onboards.updateOnboard(1);
          }

          GameObject closestVoxel = FindClosestVoxel(detector.Position);
          float closeDis = 0;
          float noSpawnRange = 0;

          if (closestVoxel != null) {
            Vector3 closePos = closestVoxel.transform.position;
            closeDis = Vector3.Distance(closePos, detector.Position);
            noSpawnRange = closestVoxel.transform.Find("Cube").gameObject.transform.localScale.x * noSpawnRangeMultiplier;
            // Debug.Log("pos: " + closePos + ", dis: " + closeDis + ", range: " + noSpawnRange);
          }
          if (closeDis < noSpawnRange) {
            _didSpawn[i] = false;
            spawnable |= _didSpawn[i];
            movableVoxel[i] = closestVoxel;
            _distanceRef = Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
            _scaling = false;
            if (closestVoxel != null) _startScale = closestVoxel.transform.localScale;
          } else {
            _didSpawn[i] = true;
            drawState.BeginNewBlock(detector.Position);
            _clearBehavior.newBlock();
          }
        }

        if (detector.DidRelease)
        {
          if (_didSpawn[i]) drawState.FinishBlock();
          movableVoxel[i] = null;
          spawnable = true;
          _scaling = false;
        }

        if (detector.IsHolding) {
          if (!colorManager.onboards.criteria[1]) {
            colorManager.onboards.updateCriteria(1);
            colorManager.onboards.updateOnboard(1);
          }
          if (_didSpawn[i]) {
            drawState.UpdateBlock(detector.Position);
          } else {
            _scaling = true;
          }
        }
      }

      if (!spawnable && colorManager != null && colorManager.modeController.currentMode == 1 && _pinchDetectorA.IsActive && _pinchDetectorB.IsActive) {
        bool sameVoxel = true;
        // Debug.Log("Mode checked");
        for (int j = 1; j < movableVoxel.Length; j++) {
          sameVoxel &= (movableVoxel[j] != null);
          sameVoxel &= (movableVoxel[j-1] != null);
          // Debug.Log(movableVoxel[j]);
          if (!sameVoxel) break;
          sameVoxel &= (movableVoxel[j].transform == movableVoxel[j - 1].transform);
        }
        
        if (sameVoxel) {
          // Debug.Log("Same voxelS");
          // Debug.Log(movableVoxel[0].transform.localScale);
          if (_scaling) {
            if (!colorManager.onboards.criteria[2]) {
              colorManager.onboards.updateCriteria(2);
              colorManager.onboards.updateOnboard(2);
            }
            movableVoxel[0].transform.localScale = _startScale * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position) / _distanceRef;
            movableVoxel[0].transform.position = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f;
          }
        }

      }
    }

    public GameObject FindClosestVoxel(Vector3 position) {
      GameObject[] gos;
      gos = GameObject.FindGameObjectsWithTag("Voxel");
      GameObject closest = null;
      float distance = Mathf.Infinity;
      foreach (GameObject go in gos)
      {
        Vector3 diff = go.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
          closest = go;
          distance = curDistance;
        }
      }
      return closest;
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