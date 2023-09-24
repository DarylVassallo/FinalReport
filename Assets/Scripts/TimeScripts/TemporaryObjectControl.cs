using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Controls The Collider Constraints, And Gravity Of The Temporary Object
public class TemporaryObjectControl : MonoBehaviour
{
    //Object Rigidbody And Collider
    private Rigidbody rb;
    private Collider coll;

    //Objects Exit Triggers
    private List<GameObject> exits;

    //The Player
    public GameObject player;

    //List Of All The Current Past Players
    private List<GameObject> pastPlayers;

    void Start()
    {
        //Initialises The Object's Rigidbody And Collider
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        //Deactivates The Object's Gravity, And Freezes It In Place
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);

        rb.useGravity = false;
        coll.enabled = true;
    }

    void Update()
    {
        //Stores A List Of All The Current Past Players In pastPlayers
        pastPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("PastHead"));

        //Stores A List Of All The Exit Triggers In exits
        exits = new List<GameObject>(GameObject.FindGameObjectsWithTag("ExitPoint"));

        //Goes Through Every Exit Trigger In exits
        foreach (GameObject exitPoint in exits)
        {
            Debug.Log("DIST: " + Vector3.Distance(player.transform.position, exitPoint.transform.position));
            //Checks If The Distance Between The Player And The Current Exit Trigger Is Less Than 0.5
            if (Vector3.Distance(player.transform.position, exitPoint.transform.position) < 1.9f)
            {

                //The Object's Constraints Are Set As None, It's Gravity Is Enabled, And It's Collider Is Disabled
                rb.constraints = RigidbodyConstraints.None;
                rb.useGravity = true;
                coll.enabled = false;
            }

            //Goes Through Every Past Player In pastPlayers
            foreach (GameObject playerInstance in pastPlayers)
            {
                //Checks If The Distance Between The Current Past Player And The Current Exit Trigger Is Less Than 0.8 
                if (Vector3.Distance(playerInstance.transform.position, exitPoint.transform.position) < 1.9f)
                {
                    //The Object's Constraints Are Set As None, It's Gravity Is Enabled, And It's Collider Is Disabled
                    rb.constraints = RigidbodyConstraints.None;
                    rb.useGravity = true;
                    coll.enabled = false;
                }
            }
        }
    }
}
