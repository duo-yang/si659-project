using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Leap;
using Leap.Unity;

public class Onboarding : MonoBehaviour
{
  public float Interval = .1f; //seconds
  public TextMesh ClapperInst;
  public static int count = 0;

  public UnityEvent OnClap;

  private LeapProvider provider = null;
  private bool velocityThresholdExceeded = false;

  public float Proximity = 0.1f; //meters
  public float VelocityThreshold = 0.1f; //meters/s
  public float PalmAngleLimit = 75; //degrees


  public GameObject[] onboarding;
  public int curInst = 0;
  private int _instLimit;


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
    _instLimit = onboarding.Length;
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



  public void updateInst() {
    if (curInst < _instLimit) {
      onboarding[curInst++].SetActive(false);
      if (curInst < _instLimit) {
        onboarding[curInst].SetActive(true);
      }
    }
  }

}