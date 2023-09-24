using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

//This Class Stops The Reset Mechanic One The Player Has Moved To The Centre
public class MoveToResetCentre : MonoBehaviour
{
    [Header("PLAYER")]
    //Player's Camera
    private Transform player;

    private Transform playerBody;

    //Reference To The Player's Rigidbody Component
    private Rigidbody rb;

    [Header("RECORD ROTATION POSITION")]
    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    [Header("TOGGLEAMPLIFIER")]
    //Reference To The ToggleAmplifier Class
    private ToggleAmplifier toggle;

    //Stores The Distance Between The Player and The Exit Ring
    private float distanceBetween;

    [Header("ADD EXISTING CORNERS")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    [SerializeField]
    //Minimum Distance Player Needs To Be With The Reset Ring To Stop The Reset Mechanic
    private float radius;

    [HideInInspector]
    //Stores All Existing Walls in the Level
    public GameObject[] walls;

    [HideInInspector]
    //Stores All Existing Interactable Objects In the Level
    public GameObject[] interactableObjects;

    [SerializeField]
    //Shows If The Reset Mechanic Can Be Used
    private bool canPlayerReset;

    //Reference To The ShouldPlayerFall Class
    private ShouldPlayerFall playerFall;

    // Start is called before the first frame update
    void Start()
    {
        //Player's Camera
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //OVRCamera Object
        playerBody = GameObject.FindGameObjectWithTag("OVRCameraRig").transform;

        //Reference To The Player's Rigidbody
        rb = GameObject.FindGameObjectWithTag("MainPlayerBody").GetComponent<Rigidbody>();

        //Reference To The ShouldPlayerFall Class
        playerFall = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<ShouldPlayerFall>();

        //Reference To The RecordRotationPosition Class
        record = GameObject.FindGameObjectWithTag("PlayerControl").GetComponent<RecordRotationPosition>();

        //Reference To The AddexistingCorners class
        add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Reference To The ToggleAmplifier
        toggle = GameObject.FindGameObjectWithTag("Toggle").GetComponent<ToggleAmplifier>();

        //Sets The Position And Scale Of The Movement Ring
        transform.position = new Vector3(0, player.position.y - 0.5f, 0);
        transform.localScale = new Vector3(toggle.radius * 2, transform.localScale.y, toggle.radius * 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Keeps The Reset Ring In Line With The Player's Waist
        transform.position = new Vector3(0, player.position.y - 0.5f, 0);

        //Enables Gravity For The Player
        rb.isKinematic = true;
        rb.useGravity = false;

        //Calculates The Distance Betwen The Player And The Reset Ring
        distanceBetween = Vector3.Distance(new Vector3(player.position.x, 0, player.position.z),
                                            new Vector3(transform.position.x, 0, transform.position.z));

        //Reference To The ToggleAmplifier
        toggle = GameObject.FindGameObjectWithTag("Toggle").GetComponent<ToggleAmplifier>();

        //Checks If The Player Is Within The Range On The X And Z Axis
        if (distanceBetween <= toggle.radius)
        {
            record.rightHandGrabber.SetActive(true);
            record.leftHandGrabber.SetActive(true);

            //Checks If The Player Should Use The Reset Mechanic
            if (canPlayerReset == true)
            {
                //Reference To The AddexistingCorners class
                add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

                //Shows That The Reset Mechanic Will Stop
                add.isResetActivated = false;
            }

            //Makes The Walls, Floor, And Interactable Objects Visible
            add.ground.SetActive(true);

            foreach (GameObject wall in walls)
            {
                wall.GetComponent<Renderer>().enabled = true;
            }

            foreach (GameObject intObject in interactableObjects)
            {
                intObject.GetComponent<Renderer>().enabled = true;
            }

            //Checks If The Player Should Use The Reset Mechanic
            if (canPlayerReset == true)
            {
                //Triggers The Reset Mechanic 
                record.resetTrigger = true;
            }

            
            GameObject.FindGameObjectWithTag("MainPlayerBody").transform.position = new Vector3(0, 5f, 0);
            playerBody.position = new Vector3(playerBody.position.x, 5f, playerBody.position.z);

            //Enables Gravity For The Player
            rb.isKinematic = false;
            rb.useGravity = true;
            playerFall.fallingAmount = -100;

            //Disables The Reset Ring
            this.gameObject.SetActive(false);
        }
    }   
}