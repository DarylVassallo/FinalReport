using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

//This Class Allows The Player And Interactable Objects To Change Universe
public class ChangeUniverse : MonoBehaviour
{
    [Header("UNIVERSE NUMBERS")]
    //Stores The Number Of The Visible Universe
    private int visibleUniverse;

    [HideInInspector]
    //Stores The Number Of The Universe Shown Through The Portal
    public int hiddenUniverse;

    [Header("PLAYERUNIVERSETRACKER")]
    //References The PlayerUniverseTracker Class
    private PlayerUniverseTracker playerUniverse;

    //Shows The Amount Of Iterations Since An Object Used The Portal
    private int intObjectChangedWait;

    // Start is called before the first frame update
    void Start()
    {
        //Sets intObjectChangedWait to 0
        intObjectChangedWait = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Increments intObjectChangedWait If It Is Less Than 20
        if (intObjectChangedWait < 20)
        {
            intObjectChangedWait++;
        }
    }

    //This Function Allows The Portal To Show The Correct Universe
    public void setUp(int hidden)
    {
        //Sets The Visible Universe To The Player's Current Universe, And Sets The Hidden Universe To Be Equal To hidden
        playerUniverse = GameObject.FindWithTag("MainPlayerBody").GetComponent<PlayerUniverseTracker>();

        visibleUniverse = playerUniverse.currentUniverse;
        hiddenUniverse = hidden;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks If The Player's Camera Is Colliding With The Portal
        if (other.name == "CenterEyeAnchor")
        {
            //Sets The Player To Exist In the Hidden Universe
            playerUniverse.currentUniverse = hiddenUniverse;

            //If The Player Was Holding An Interactable Object When Colliding With the Portal,
            //the Object Is Set To Exist In the Hidden Universe. It Is Also Shown That None Of The Interactable Object Needs To Be Checked
            GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("InteractableObject");
            foreach (GameObject intObject in interactableObjects)
            {
                Grabbable grabbing = intObject.GetComponent<Grabbable>();

                if(grabbing.isBeingGrabbed == true)
                {
                    ChangeObjectLayer change = intObject.GetComponent<ChangeObjectLayer>();
                    change.currentUniverse = hiddenUniverse;
                }
            }

            //Removes All Existing Portals
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach (GameObject portal in portals)
            {
                Destroy(portal);
            }
        }

        //If An Interactable Object Is Colliding With the Portal, Then It Is Shown That The Object Is Being Checked
        if(other.tag == "InteractableObject")
        {
            //Checks If 20 Iterations Have Passed Since An Object Crossed Through The Portal
            if (intObjectChangedWait >= 20)
            {
                ChangeObjectLayer change = other.gameObject.GetComponent<ChangeObjectLayer>();

                //Checks If The Object Exists In The Visible Universe
                if (change.currentUniverse == visibleUniverse)
                {
                    //The Object Is Set To Exist In The Hidden Universe
                    change.currentUniverse = hiddenUniverse;
                }
                else
                {
                    //The Object Is Set To Exist In The Visible Universe
                    change.currentUniverse = visibleUniverse;
                }

                //Sets intObjectChangedWait To 0
                intObjectChangedWait = 0;
            }
        }

    }
}
