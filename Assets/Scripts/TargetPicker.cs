using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetPicker : MonoBehaviour
{


    public Transform rightHand,leftHand;
    public ParticleSystem aimLinePS;
    public ParticleSystem hitObjectPS;
    public MultiView multiView;
    public Grabber grabber;
    public ScanningMeshVFX scanningMeshVFX;

    const int mask = -10;
    // Start is called before the first frame update
    void Start()
    {
        var a =  Input.GetJoystickNames();
        foreach (var item in a)
        {
            Debug.Log(a);
        }
    }


    bool holding;
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.EnablePicking) return;

        //if (Input..Get(OVRInput.Button.SecondaryIndexTrigger))
        //    {
        //        aimLinePS.Play();
        //    }
        //    if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        //    {

        //        CastRay();
        //    }
    }

    void CastRay()
    {
        aimLinePS.Play();
        Debug.DrawRay(rightHand.position, rightHand.forward * 10);
        if(Physics.Raycast(rightHand.position,rightHand.forward,out RaycastHit hit, -mask))
        {
            Debug.DrawRay(hit.point, Vector3.up) ;
            if(hit.transform.CompareTag("Interactable"))
            {
                hitObjectPS.transform.position = hit.point;
                hitObjectPS.Play();
                var mf = hit.transform.GetComponent<MeshFilter>();
                scanningMeshVFX.mesh = mf.sharedMesh;
                scanningMeshVFX.Set();
                TechDrawingCreator.instance.Create(mf.mesh, hit.transform);
                grabber.ChangeGrabber( hit.transform, hit.rigidbody);
            }
        }


    }
}
