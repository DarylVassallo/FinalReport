using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
using Oculus.Interaction;

//This Class Changes The Layer of The Object To Match The Universe It Is In
public class ChangeObjectLayer : MonoBehaviour
{
    [Header("UNIVERSE")]
    //Shows The Current Universe Number
    public int currentUniverse;

    //Reference To The PlayerUniverseTracker Class
    private PlayerUniverseTracker playerUniverse;

    //Reference To The LayerNumbers Class
    private LayerNumbers layers;

    //Shows If The Required Layer Has Been Found
    private bool foundLayer;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The PlayerUniverseTracker Class
        playerUniverse = GameObject.FindWithTag("MainPlayerBody").GetComponent<PlayerUniverseTracker>();

        //Reference To The LayerNumbers Class
        layers = GameObject.FindWithTag("Level").GetComponent<LayerNumbers>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets All Existing Portals In The Level 
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");

        //Checks If There Are Any Existing Portals
        if (portals.Length == 0)
        {
            //Sets The Object To The Visible Layer If The Object Exists Within The Same Universe As The Player
            if (currentUniverse == playerUniverse.currentUniverse)
            {
                this.gameObject.layer = layers.showOutsidePortal;

            //Otherwise Sets The Object To The Hidden Layer
            }
            else{
                this.gameObject.layer = layers.showNowhere;
            }
        }else{
            
            //Shows That The Object Has Not Found A Layer Yet
            foundLayer = false;

            //Goes Through All Existing Portal
            for (int i = 0; i < portals.Length; i++)
            {
                //References The Current Portal's ChangeUniverse Class
                ChangeUniverse changeUniverse = portals[i].GetComponent<ChangeUniverse>();

                //Sets The Object To The Hidden Layer If A Layer Has Not Been Found
                if(foundLayer == false)
                {
                    this.gameObject.layer = layers.showNowhere;
                }

                //Checks If The Object Exists Within The Same Universe As The Player 
                if (currentUniverse == playerUniverse.currentUniverse)
                {
                    //Checks If The Object Is An Interactable Object
                    if(this.gameObject.tag == "InteractableObject")
                    {
                        //Player's Camera
                        GameObject playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

                        //References The Grabbable Class
                        Grabbable grabbing = GetComponent<Grabbable>();

                        //Sets The Object To The Default Layer:
                        //If The Distance Between The Player And The Object Is Less Or Equal To
                        //The Distance Between The Player And The Current Portal,
                        //Or If The Object Is Currently Geing Grabbed
                        if ((Vector3.Distance(playerCamera.transform.position, transform.position) <= 
                            Vector3.Distance(playerCamera.transform.position, portals[i].transform.position)) || grabbing.isBeingGrabbed == true)
                        {
                            this.gameObject.layer = 0;

                        //Otherwise The Object Is Set To The Visible Layer
                        }else{
                            this.gameObject.layer = layers.showObjectOutsidePortal;
                        }

                    //Otherwise The Object Is Set To The Visible Layer
                    }
                    else{
                        this.gameObject.layer = layers.showOutsidePortal;
                    }

                    //Shows That The Object Has Found A Layer To Use
                    foundLayer = true;

                //Sets The Object To Be Visible Through Portal1, And Shows That The Object Has Found A Layer to Use:
                //If The Portal Is Named "Portal1(Clone)", And The Object Exists In The Universe Seen Through The Portal
                }else
                if (currentUniverse == changeUniverse.hiddenUniverse && portals[i].name == "Portal1(Clone)")
                {
                    this.gameObject.layer = layers.showInPortal1;
                    foundLayer = true;

                //Sets The Object To Be Visible Through Portal2, And Shows That The Object Has Found A Layer to Use:
                //If The Portal Is Named "Portal2(Clone)", And The Object Exists In The Universe Seen Through The Portal
                }
                else
                if (currentUniverse == changeUniverse.hiddenUniverse && portals[i].name == "Portal2(Clone)")
                {
                    this.gameObject.layer = layers.showInPortal2;
                    foundLayer = true;
                }
            }
        }

        //Checks If The Object Is An Interactable Object
        if (this.gameObject.tag == "InteractableObject")
        {
            //Disables The Objects Collider, If The Object Is Hidden
            if (this.gameObject.layer == layers.showNowhere)
            {
                //Reference To The Rigidbody Component Of The Object
                Rigidbody rbObject = this.gameObject.GetComponent<Rigidbody>();

                //Disables The Object's Gravity
                rbObject.useGravity = false;
                rbObject.isKinematic = true;

                //The Object's Velocity And Angular Velocity Are Set To 0
                rbObject.velocity = new Vector3(0, 0, 0);
                rbObject.angularVelocity = new Vector3(0, 0, 0);
            }
            else
            {
                //Reference To The Rigidbody Component Of The Object
                Rigidbody rbObject = this.gameObject.GetComponent<Rigidbody>();

                //Disables The Object's Gravity
                rbObject.useGravity = true;
                rbObject.isKinematic = false;
            }
        }
    }
}
