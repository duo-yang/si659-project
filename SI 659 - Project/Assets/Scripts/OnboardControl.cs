using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Leap;
using Leap.Unity;
using Leap.Unity.Animation;

public class OnboardControl : MonoBehaviour
{
  public float Interval = .1f; //seconds
  public static int count = 0;

  public UnityEvent OnClap;

  private LeapProvider provider = null;
  private bool velocityThresholdExceeded = false;

  public float Proximity = 0.1f; //meters
  public float VelocityThreshold = 0.1f; //meters/s
  public float PalmAngleLimit = 75; //degrees

#if UNITY_EDITOR
  //Debugging variables --set Inspector to debug mode
  private float currentAngle = 0;
  private float currentVelocityVectorAngle = 0;
  private float currentDistance = 0;
  private float currentVelocity = 0;
#endif

  void Start()
  {
    provider = GetComponentInParent<LeapServiceProvider>();
    _numOnboard = onboards.Length - 1;
    criteria = new bool[_numOnboard];
    for (int i = 0; i < _numOnboard; i++) {
      criteria[i] = false;
    }
    onboards[0].PlayForward();
  }

  void Update()
  {
    Hand thisHand;
    Hand thatHand;
    Frame frame = provider.CurrentFrame;
    if (frame != null && frame.Hands.Count >= 2)
    {
      thisHand = frame.Hands[0];
      thatHand = frame.Hands[1];
      if (thisHand != null && thatHand != null)
      {
        Vector velocityDirection = thisHand.PalmVelocity.Normalized;
        Vector otherhandDirection = (thisHand.PalmPosition - thatHand.PalmPosition).Normalized;

#if UNITY_EDITOR
        //for debugging
        Debug.DrawRay(thisHand.PalmPosition.ToVector3(), velocityDirection.ToVector3());
        Debug.DrawRay(thatHand.PalmPosition.ToVector3(), otherhandDirection.ToVector3());
        currentAngle = thisHand.PalmNormal.AngleTo(thatHand.PalmNormal) * Constants.RAD_TO_DEG;
        currentDistance = thisHand.PalmPosition.DistanceTo(thatHand.PalmPosition);
        currentVelocity = thisHand.PalmVelocity.MagnitudeSquared + thatHand.PalmVelocity.MagnitudeSquared;
        currentVelocityVectorAngle = velocityDirection.AngleTo(otherhandDirection) * Constants.RAD_TO_DEG;
#endif

        if (thisHand.PalmVelocity.MagnitudeSquared + thatHand.PalmVelocity.MagnitudeSquared > VelocityThreshold &&
          velocityDirection.AngleTo(otherhandDirection) >= (180 - PalmAngleLimit) * Constants.DEG_TO_RAD)
        {
          velocityThresholdExceeded = true;
        }
      }
    }
  }

  void OnEnable()
  {
    StartCoroutine(clapWatcher());
  }

  void OnDisable()
  {
    StopCoroutine(clapWatcher());
  }

  IEnumerator clapWatcher()
  {
    Hand thisHand;
    Hand thatHand;
    while (true)
    {
      if (provider)
      {
        Frame frame = provider.CurrentFrame;
        if (frame != null && frame.Hands.Count >= 2)
        {
          thisHand = frame.Hands[0];
          thatHand = frame.Hands[1];
          if (thisHand != null && thatHand != null)
          {
            //decide if clapped
            if (velocityThresholdExceeded && //went fast enough
                      thisHand.PalmPosition.DistanceTo(thatHand.PalmPosition) < Proximity && // and got close 
                      thisHand.PalmNormal.AngleTo(thatHand.PalmNormal) >= (180 - PalmAngleLimit) * Constants.DEG_TO_RAD) //while facing each other
            {
              OnClap.Invoke();
            }
          }
        }
      }
      velocityThresholdExceeded = false;
      yield return new WaitForSeconds(Interval);
    }
  }

  public TransformTweenBehaviour[] onboards;
  public float tweenDelay;
  public bool[] criteria;
  public int currentOnboard = 0;
  private int _numOnboard;

  public void updateOnboard(int onboardIndex) {
    // Debug.Log(onboardIndex + ", " + currentOnboard);
    if (onboardIndex == currentOnboard && meetCriteria(onboardIndex) && onboardIndex < _numOnboard) {
      onboards[onboardIndex].PlayBackward();
      onboards[onboardIndex + 1].PlayForwardAfterDelay(tweenDelay);
      currentOnboard++;
      Debug.Log("current onboard: " + currentOnboard + ", onboard index: " + onboardIndex);
    }
  }

  public void updateCriteria(int onboardIndex) {
    switch (onboardIndex)
    {
      case 0: // welcome
        criteria[onboardIndex] = true;
        break;
      case 1: // pinch
        criteria[onboardIndex] = true;
        break;
      case 2: // scale in create mode
        criteria[onboardIndex] = true;
        break;
      case 3: // hand UI
        criteria[onboardIndex] = true;
        break;
      case 4: // color picker
        criteria[onboardIndex] = true;
        break;
      case 5: // switch mode
        criteria[onboardIndex] = true;
        break;
      case 6: // buttons
        criteria[onboardIndex] = true;
        break;
      case 7: // outro
        criteria[onboardIndex] = true;
        break;
      default:
        break;
    }
  }

  private bool meetCriteria(int onboardIndex) {
    bool criteriaMet = false;
    switch (onboardIndex) {
      case 0: // welcome
        criteriaMet |= criteria[onboardIndex];
        break;
      case 1: // pinch
        criteriaMet |= criteria[onboardIndex];
        break;
      case 2: // scale in create mode
        criteriaMet |= criteria[onboardIndex];
        break;
      case 3: // hand UI
        criteriaMet |= criteria[onboardIndex];
        break;
      case 4: // color picker
        criteriaMet |= criteria[onboardIndex];
        break;
      case 5: // switch mode
        criteriaMet |= criteria[onboardIndex];
        break;
      case 6: // buttons
        criteriaMet |= criteria[onboardIndex];
        break;
      case 7: // outro
        criteriaMet |= criteria[onboardIndex];
        break;
      default:
        criteriaMet = true;
        break;
    }
    return criteriaMet;
  }

}