using UnityEngine;

public class GrabberCollider : MonoBehaviour
{
    public Grabber grabber;
    public Grabber.side side;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            other.transform.parent.SendMessage("OnButtonPress");

        }else if(GameManager.EnableRotating && other.CompareTag("Interactable"))
        {
            grabber.grabbed = other.transform;
            grabber.OnReach(side);

        }
    }


    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform == grabber.grabbed)
        {
            grabber.OnReach(side);
            grabber.grabbedRB = other.attachedRigidbody;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == grabber.grabbed)
        {
            switch (side)
            {
                case Grabber.side.right:
                    grabber.touchedR = false;
                    break;
                case Grabber.side.left:
                    grabber.touchedL = false;
                    break;
                default:
                    break;
            }
        }
    }


}
