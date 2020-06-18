using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public static Grabber instance;
    public Transform grabbed;
    public Rigidbody grabbedRB;
    public Transform rightH, leftH;
    public Transform childR, childL;
    public bool holdR, holdL;
    public bool touchedR, touchedL;
    bool GrabbingLever = true;
    public float pullSpeed = 1;
    public enum side { right, left }

    public void ChangeGrabber(Transform t,Rigidbody r)
    {
        grabbed = t;
        grabbedRB = r;
        touchedL = false;
        touchedR = false;
    }

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        childR = new GameObject("ChildR").transform;
        childL = new GameObject("ChildL").transform;
        childR.SetParent(rightH);
        childL.SetParent(leftH);

    }
    public void OnLeverTouch(side s, Transform go)
    {
        if (s == side.left)
        {
           // if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
           // {
                go.GetComponent<LeverAction>().Grab(leftH);
                GrabbingLever = true;
                touchedL = false;
                touchedR = true;
                grabbed = go;
            grabbedLever = null;
          //  }
        }
        else if (s == side.right)
        {
            //if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
           // {
                go.GetComponent<LeverAction>().Grab(rightH);
                GrabbingLever = true;
                grabbed = go;
                touchedR = true;
                touchedL = false;
            grabbedLever = null;
           // }
        }
    }

    public void OnReach(side s)
    {
  

        if ((!touchedL &&holdL && s == side.left) || (!touchedR&&holdR && s == side.right))
        {
             FixObject(s);
        }


    }

    public void FixObject(side s)
    {
                GrabbingLever = false;
        switch (s)
        {
            case side.right:
                childR.position = grabbed.position;
                childR.rotation = grabbed.rotation;
                touchedR = true;
                //grabDirR = childR.position - rightH.position;
                //grabDirR = childR.InverseTransformVector(grabDirR);

                break;
            case side.left:
                childL.position = grabbed.position;
                childL.rotation = grabbed.rotation;
                touchedL = true;
                //grabDirL = childL.position - leftH.position;
                //grabDirL= childL.InverseTransformVector( grabDirL);
                break;
        }
    }
    LeverAction grabbedLever;
    private void Update()
    {



        if ((!GameManager.EnableGrabbing && !GameManager.EnableRotating )|| grabbed== null) return;
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            if (!GrabbingLever)
            {
                if (!holdR && touchedR) FixObject(side.right);
                if (touchedR)
                {
                    
                    grabbed.position = childR.position;
                    grabbed.rotation = childR.rotation;
                    grabbedRB.velocity = Vector3.zero;
                    grabbedRB.angularVelocity = Vector3.zero;

                    grabbed.rotation = childR.rotation;

                }
                else if (!GameManager.EnableRotating)
                {
                    //force pull
                    Vector3 force = (rightH.position - grabbed.position).normalized * pullSpeed;
                    grabbed.position += (force * Time.deltaTime);
                    grabbedRB.velocity *= 0.9f;

                }
            }
            holdR = true;
        } else if (holdR ) { holdR = false;
            if (GrabbingLever && touchedR)
            {
                grabbed.SendMessage("Release");
            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            if (!GrabbingLever)
            {
                if (!holdL && touchedL) FixObject(side.left);
                if (touchedL)
                {

                    grabbed.position = childL.position;

                    grabbed.rotation = childL.rotation;
                    grabbedRB.velocity = Vector3.zero;
                    grabbedRB.angularVelocity = Vector3.zero;

                    grabbed.rotation = childL.rotation;


                }
                else if (!GameManager.EnableRotating)
                {
                    //force pull
                    Vector3 force = (leftH.position - grabbed.position).normalized * pullSpeed;
                    grabbed.position += (force * Time.deltaTime);
                    grabbedRB.velocity *= 0.9f;
                }
            }
            holdL = true;

        }
        else if (holdL) { holdL = false;
            if (GrabbingLever && touchedL)
            {
                grabbed.SendMessage("Release");
            }
        }
    }
}
