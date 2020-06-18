using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using OculusSampleFramework;

public class TargetPicker : MonoBehaviour
{


    public Transform rightHand,leftHand;
    public ParticleSystem ps;
    public ParticleSystem HitPs;
    public MultiView multiView;
    public Grabber grabber;
    public ScanningMeshVFX scanningMeshVFX;

    const int mask = -10;
    // Start is called before the first frame update
    void Start()
    {
    }


    bool holding;
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.EnablePicking) return;

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                ps.Play();
            }
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {

                CastRay();
            }
        
        //}else if (holding)
        //{
        //    if (multiView.target != null)
        //    {
        //        multiView.target.position = lcontroller.position;
        //        multiView.target.rotation = lcontroller.rotation;
        //        Vector3 vel = OVRInput.GetLocalControllerVelocity(controller).normalized;
        //        multiView.targetRB.isKinematic = false;

            //        holding = false;
            //    }
            //}

    }

    void CastRay()
    {
        ps.Play();
        Debug.DrawRay(rightHand.position, rightHand.forward * 10);
        if(Physics.Raycast(rightHand.position,rightHand.forward,out RaycastHit hit, -mask))
        {
            Debug.DrawRay(hit.point, Vector3.up) ;
            if(hit.transform.CompareTag("Interactable"))
            {
                HitPs.transform.position = hit.point;
                HitPs.Play();
                var mf = hit.transform.GetComponent<MeshFilter>();
                scanningMeshVFX.mesh = mf.sharedMesh;
                scanningMeshVFX.Set();
                TechDrawingCreator.instance.Create(mf.mesh, hit.transform);
                grabber.ChangeGrabber( hit.transform, hit.rigidbody);
            }
        }


    }
}
