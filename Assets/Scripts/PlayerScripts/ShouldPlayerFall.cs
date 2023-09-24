using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Detects When To Apply Gravity to the Player
public class ShouldPlayerFall : MonoBehaviour
{
    [Header("ADD EXISTING CORNERS")]
    //Reference To The AddexistingCorners class
    private AddExistingCorners add;

    [Header("RIGIDBODY")]
    //Player's Rigidbody
    private Rigidbody rb;

    [HideInInspector]
    //Checks If the Player Has Fallen Off The Platform
    public int fallingAmount;

    // Start is called before the first frame update
    void Start()
    {
        //Checks If the AddExistingCorners Class Exists In The Level
        if (GameObject.FindGameObjectWithTag("CornerSetter") != null)
        {
            //Reference To The AddexistingCorners class
            add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();
        }

        //Player's Rigidbody
        rb = GameObject.FindGameObjectWithTag("MainPlayerBody").GetComponent<Rigidbody>();

        //Allows The Player To Fall Onto A Platform
        fallingAmount = -100;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks If The Level Uses The AddExistingCorners Class
        if (add != null)
        {
            //Disables The Player's Gravity If The Reset Mechanic Is Being Used
            if (add.isResetActivated == true)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        //Player's Rigidbody
        rb = GameObject.FindGameObjectWithTag("MainPlayerBody").GetComponent<Rigidbody>();

        //Increments fallingAmount If The Player Is Currently Falling
        if (rb.useGravity == true)
        {
            fallingAmount++;
        }

        //Resets fallingAmount If The Player Has Fallen Below the Level
        if(GameObject.FindGameObjectWithTag("MainPlayerBody").transform.position.y <= -5)
        {
            fallingAmount = -100;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Checks If The Level Uses The AddExistingCorners Class
        if (add != null)
        {
            //Disables The Player's Gravity If The Reset Mechanic Is Not Being Used And fallingAmount Is Less Or Equal To 40
            if (add.isResetActivated == false && fallingAmount <= 40)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                fallingAmount = 0;
            }
        }
        else
        {
            //Disables The Player's Gravity If fallingAmount Is Less Or Equal To 40
            if (fallingAmount <= 40)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                fallingAmount = 0;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Checks If The Level Uses The AddExistingCorners Class
        if (add != null)
        {
            //Enables The Player's Gravity If The Reset Mechanic Is Not Being Used
            if (add.isResetActivated == false)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
        else
        {
            //Enables The Player's Gravity
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
