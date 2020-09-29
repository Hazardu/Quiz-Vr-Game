using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverAction : MonoBehaviour
{
    public AudioSource source;
    public bool toggled;
    public bool isGrabbed;
    public bool CanBeGrabbed;
    public Grabber grabber;
    public float downAngleTreshold;
    public float maxDistance,minDistance;
    private float rotation;
    private Transform hand;
    public Transform handle;
    public AnswerManager answerManager;
    public Piedestal piedestal;
    public Interaction interaction;
    private Transform interactionTr;
    public UnityEvent onPulled;
    public bool awaitReset;
    // Start is called before the first frame update
    void Start()
    {
        answerManager = AnswerManager.instance;
        maxDistance *= maxDistance;
        minDistance *= minDistance;
        interactionTr = interaction.transform;

        if (grabber == null)
            grabber = Grabber.instance;
    }
    OVRInput.Button btnToTest;
    
    bool StillGrabbing()
    {
        return OVRInput.Get(btnToTest);
    }
    void TestGrabbing()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            float mag = (interactionTr.position - PlayerHands.right.position).sqrMagnitude;
            if (mag < interaction.innerRange)
            {
                btnToTest = OVRInput.Button.SecondaryHandTrigger;
                Grab(PlayerHands.right);
                return;
            }
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            float mag = (interactionTr.position - PlayerHands.left.position).sqrMagnitude;

            if (mag < interaction.innerRange)
            {
                btnToTest = OVRInput.Button.PrimaryHandTrigger;

                Grab(PlayerHands.left);
                return;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!isGrabbed && CanBeGrabbed)
        {
            TestGrabbing();
        }

        if (isGrabbed && !toggled)
        {
            Vector3 handDir = handle.position - hand.position;
            if (handDir.sqrMagnitude > maxDistance || handDir.sqrMagnitude < minDistance || !StillGrabbing())
            {
                Release();
                return;
            }
            //if (Vector3.Dot(handDir,transform.forward) < 0)
            //{
            //    Debug.Log("Dot incorrect");
            //    Release();
            //    return;
            //}


            handDir.Normalize();
            var r = Quaternion.LookRotation(-handDir);
            r.eulerAngles = new Vector3(r.eulerAngles.x, -90, 90);
            handle.localRotation = r;
            //if distance is bigger than max dist, release
            if (Vector3.Angle(handle.forward, -transform.up) < downAngleTreshold)
            {
                toggled = true;
                Release();
                StartCoroutine(ResetLeverAsync());
            }

        }
      
    }

    public void  ToggleGrabable(bool b)
    {
        CanBeGrabbed = b;
    }


    public void Grab(Transform handTr)
    {

        if (isGrabbed || toggled)
        {
            return;
        }
            isGrabbed = true;
        hand = handTr;
    }
    public void Release()
    {
        isGrabbed = false;
        grabber.grabbed = null;
        
    }


    public IEnumerator LeverPulledAsync()
    {
        var r = handle.transform.localRotation.eulerAngles.x;
        float t = 0;
        Debug.Log("Snapping rotation");
        while (t < 1)
        {
            handle.localRotation = Quaternion.Euler(Mathf.Lerp(r,90, t), -90, 90);
            t += Time.deltaTime*5;
            yield return null;
        }
        Debug.Log("Rot snapped");
        yield return new WaitForSeconds(1);
        piedestal?.OnLeverPulled();
        onPulled?.Invoke();
        Debug.Log("Invoking submit");
        if (!awaitReset)
        {
            yield return ResetLeverAsync();
        }

    }
    public void ResetLever()
    {
        StartCoroutine(ResetLeverAsync());
    }
    IEnumerator ResetLeverAsync()
    {
        float t = 0;
        while (t < 1)
        {
            handle.localRotation = Quaternion.Euler(Mathf.Lerp(90, -90, t), -90, 90);
            t += Time.deltaTime * 0.4f;
            yield return null;
        }
        handle.transform.localRotation = Quaternion.Euler(-90, -90, 90);
        toggled = false;
    }

}
