using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Causes The Object To Fall After A Certain Period Of Time
public class WaitToFall : MonoBehaviour
{
    [Header("OBJECT INTERACTION")]
    //Object Rigidbody And Collider
    private Rigidbody rb;
    private Collider coll;

    public RecordRotationPosition record;
    void Start()
    {
        //Initialises The Object's Rigidbody And Collider
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        //Deactivates The Object's Gravity
        rb.useGravity = false;
    }

    void Update()
    {
        //Stops The Object From Falling:
        //If The Rewind Mechanic Is Being Used
        if (record.rewindTrigger == true)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.useGravity = false;
            coll.enabled = true;
        }
        else
        {
            //Checks If There Are Past Players In The Level
            if (record.attempts.Count > 0)
            {
                //Checks If The Object Should Fall
                if (record.headPosition.Count > record.attempts[record.attempts.Count - 1] && record.headPosition.Count <= (record.attempts[record.attempts.Count - 1] + 50))
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.angularVelocity = new Vector3(0, 0, 0);
                    rb.useGravity = false;
                    coll.enabled = true;
                }
                else
                if (record.headPosition.Count > record.attempts[record.attempts.Count - 1] && record.headPosition.Count > (record.attempts[record.attempts.Count - 1] + 50))
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.useGravity = true;
                    coll.enabled = false;
                }
            }
            else
            {
                //Checks If The Object Should Fall
                if (record.headPosition.Count <= 50)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.angularVelocity = new Vector3(0, 0, 0);
                    rb.useGravity = false;
                    coll.enabled = true;
                }
                else
                if (record.headPosition.Count > 50)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.useGravity = true;
                    coll.enabled = false;
                }
            }
        }
    }
}
