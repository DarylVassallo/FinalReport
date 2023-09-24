using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Enables And Disables The Objects Gravity
public class objectGravity : MonoBehaviour
{
    [Header("RIGIDBODY")]
    //Reference To The Rigidbody Class
    private Rigidbody rb;

    [Header("RECORDROTATIONPOSITION")]
    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    [Header("HAND CHECKING")]
    //Shows Which Hand Was Used To Grab The Object
    private int whichHandGrabbed;

    //Shows If The Object Was Recently Grabbed
    private bool justGrabbed;

    //Shows The Number Of Iterations Left Checking Which Hand Grabbed The Object
    private int justGrabbedIteration;

    [Header("AVERAGE VELOCITY AND ANGULAR VELOCITY")]
    //Stores The Average Velocity Of The Object
    private Vector3 meanVelocity;

    //Stores The Average Angular Velocity Of The Object
    private Vector3 meanAngularVelocity;

    void Start()
    {
        //Reference To The Rigidbody Class
        rb = GetComponent<Rigidbody>();

        //Reference To The RecordRotationPosition Class
        record = GameObject.FindWithTag("PlayerControl").GetComponent<RecordRotationPosition>();

        whichHandGrabbed = 0;
        justGrabbed = false;
        justGrabbedIteration = 200;
    }

    void Update()
    {
        //Checks If The Object Was Recently Grabbed, And If There Are Any Iterations Left To Check Which Hand Was Used
        if (justGrabbed == true && justGrabbedIteration > 0)
        {
            //Stops Checking The Hand Used, And Shows That The Right Hand Was Used
            if (record.rightHandGrabAPI.publicAnyHolding)
            {
                whichHandGrabbed = 1;
                justGrabbed = false;
                justGrabbedIteration = 1;

            //Stops Checking The Hand Used, And Shows That The Left Hand Was Used
            }else if (record.leftHandGrabAPI.publicAnyHolding)
            {
                whichHandGrabbed = 2;
                justGrabbed = false;
                justGrabbedIteration = 1;
            }

            //Decrements The Amount Of Iterations Left To Check Which Hand Was Used
            justGrabbedIteration--;

            //Resets justGrabbed And justGrabbedIteration If There Are No Iterations Left
            if(justGrabbedIteration == 0)
            {
                justGrabbed = false;
                justGrabbedIteration = 200;
            }
        }
    }

    //This Function Disables The Object's Gravity
    public void objectGrabbed()
    {
        rb.useGravity = false;
        rb.isKinematic = true;

        justGrabbed = true;
        justGrabbedIteration = 200;
    }

    //This Function Enables The Object's Gravity, Applies The Correct Velocity, And Angular Velocity
    public void objectReleased()
    {
        //Enables The Object's Gravity
        rb.useGravity = true;
        rb.isKinematic = false;

        //Checks If The Right Hand Was Holdiing The Object
        if(whichHandGrabbed == 1)
        {
            //Calculates Right Hand's Average Velocity And Angular Velocity For The Last Ten Iterations
            for (int i = (record.rightVelocity.Count - 1 - 10); i <= (record.rightVelocity.Count - 1); i++)
            {
                meanVelocity = meanVelocity + record.rightVelocity[i];
                meanAngularVelocity = meanAngularVelocity + record.rightAngularVelocity[i];
            }

            meanVelocity = meanVelocity / 10;
            meanAngularVelocity = meanAngularVelocity / 10;

            //The Velocity, And Angular Velocity Are Set
            rb.velocity = meanVelocity;
            rb.angularVelocity = meanAngularVelocity;

            //The Variables Used Are Reset
            meanVelocity = new Vector3(0, 0, 0);
            meanAngularVelocity = new Vector3(0, 0, 0);
            whichHandGrabbed = 0;

        //Checks If The Right Hand Was Holdiing The Object
        }else if (whichHandGrabbed == 2)
        {
            //Calculates Left Hand's Average Velocity And Angular Velocity For The Last Ten Iterations
            for (int i = (record.leftVelocity.Count - 1 - 10); i <= (record.leftVelocity.Count - 1); i++)
            {
                meanVelocity = meanVelocity + record.leftVelocity[i];
                meanAngularVelocity = meanAngularVelocity + record.leftAngularVelocity[i];
            }

            meanVelocity = meanVelocity / 10;
            meanAngularVelocity = meanAngularVelocity / 10;

            //The Velocity, And Angular Velocity Are Set
            rb.velocity = meanVelocity;
            rb.angularVelocity = meanAngularVelocity;

            //The Variables Used Are Reset
            meanVelocity = new Vector3(0, 0, 0);
            meanAngularVelocity = new Vector3(0, 0, 0);
            whichHandGrabbed = 0;
        }
    }
}
