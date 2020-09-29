using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 7,gravityAcc = 2;
    public CharacterController chc;
    public Transform tr,baseTr;

    private float yMov;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(12, 11);
        Physics.IgnoreLayerCollision(12, 10);
    }

    float x, y;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleMovement();
    }
    
    void HandleMovement()
    {

        //Vector3 basePos = baseTr.transform.position;
        //transform.position = basePos;
        //baseTr.position = basePos;




        Vector3 movement = tr.forward * y + tr.right * x;

        if (!chc.isGrounded)
            yMov += Time.deltaTime * gravityAcc;
        else
        {
            yMov = 0;
        }
        movement.y = -yMov;
        chc.Move(movement * Time.deltaTime * walkSpeed);

    }

    void GetInput()
    {
       // var v2 = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //v2.Normalize();
        //x = v2.x;
        //y = v2.y;
    }
}
