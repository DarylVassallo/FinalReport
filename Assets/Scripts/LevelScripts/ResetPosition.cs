using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

//Resets The Position And Rotation Of An Object
public class ResetPosition : MonoBehaviour
{
    [Header("SPAWN POSITION")]
    //If The Object Needs, This Shows Where The Object's Spawn Position Is
    public Transform spawnPoint;

    //Used To Record The First Position And Rotation Of The Object, Which Is Then Used As Their Spawn Point
    private Vector3 spawnPosition;
    private Vector3 spawnRotation;

    [Header("OBJECT PHYSICS")]
    [SerializeField]
    private bool doNotResetIfFalling;

    //Reference To The Rigidbody Class
    private Rigidbody rb;

    //Reference To The Collider Class
    private Collider coll;

    [Header("ORIGNAL UNIVERSE")]
    [SerializeField]
    private int orignUniverse;

    [Header("RESPAWN CENTRE")]
    //Reset Ring
    public GameObject respawnCentre;

    //Reference To the MoveToResetCentre Class
    private MoveToResetCentre moving;

    [Header("LEVEL SETUP")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    private AudioSource narratorAudio;
    private AudioSource playerResetAudio;

    [Header("RECORD ROTATION POSITION")]
    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    //List For All Interactable Objects
    private List<GameObject> interactableObjects;

    //Shows If An Object Is Using A Reset Audio Clip
    private bool isAnObjectResetting;

    //Shows If The Object Has Fallen
    private bool hasObjectFallen;

    [Header("USES PORTALS")]
    public bool usesPortals;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The Rigidbody Class
        rb = this.gameObject.GetComponent<Rigidbody>();

        //Reference To The Collider Class
        coll = this.gameObject.GetComponent<Collider>();

        //Reference To The AddExistingCorners Class
        add = GameObject.FindWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Checks If The Reset Centre Object Is Available
        if (respawnCentre != null)
        {
            //Reference To The MoveToResetCentre Class
            moving = respawnCentre.GetComponent<MoveToResetCentre>();
        }

        //Sets The Spawn Postition And Rotation To That Of The Object
        spawnPosition = transform.position;
        spawnRotation = transform.eulerAngles;

        //Sets The Object's Current Universe To It's Original Universe:
        //If The Original Universe Number Is Not Set To Be 0
        if (orignUniverse != 0)
        {
            //Checks If The Object Is The Player
            if (this.gameObject.tag == "MainPlayerBody")
            {
                //Sets The Object's Current Universe To It's Original Universe
                this.gameObject.GetComponent<PlayerUniverseTracker>().currentUniverse = orignUniverse;
            }
            else
            {
                //Sets The Object's Current Universe To It's Original Universe
                this.gameObject.GetComponent<ChangeObjectLayer>().currentUniverse = orignUniverse;
            }  
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Resets The Position And Rotation Of The Object If It Has Went Below The Level
        if(transform.position.y <= -10 && doNotResetIfFalling == false)
        {
            if (this.gameObject.tag == "MainPlayerBody")
            {
                //Reference To The AddExistingCorners Class
                add = GameObject.FindWithTag("CornerSetter").GetComponent<AddExistingCorners>();

                //Stores All Past Players In The Array
                GameObject[] pastPlayers = GameObject.FindGameObjectsWithTag("PastPlayer");

                //Destroys All Existing Past Players
                for (var i = 0; i < pastPlayers.Length; i++)
                {
                    Destroy(pastPlayers[i]);
                }

                //Hides All Objects In The Level
                add.ground.SetActive(false);

                //Makes The Player Stop Grabbing
                record = GameObject.FindWithTag("PlayerControl").GetComponent<RecordRotationPosition>();
                record.rightHandGrabber.SetActive(false);
                record.leftHandGrabber.SetActive(false);

                //References The Narrator's AudioSource Component
                narratorAudio = GameObject.FindWithTag("Narrator").GetComponent<AudioSource>();

                //Stores All Interactable Objects
                interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

                isAnObjectResetting = false;

                //Checks If There Are Any Interactable Objects
                if (interactableObjects.Count > 0)
                {
                    //Goes Through All Existing Interactable Object
                    foreach (GameObject intObject in interactableObjects)
                    {
                        //References The Object's Reset AudioSource Component
                        AudioSource intObjectAudio = intObject.GetComponent<AudioSource>();

                        //Checks If The Object Is Using The Reset Audio Clip
                        if (intObjectAudio.isPlaying)
                        {
                            isAnObjectResetting = true;
                        }
                    }
                }

                //Checks If The Respawn Centre Object Is Not Active, And If The Narrator Is Not Currently Speaking
                if (respawnCentre.activeSelf == false && !narratorAudio.isPlaying && isAnObjectResetting == false)
                {

                    //Starts The Retry Audio Clip
                    AudioSource retryAudio = GetComponent<AudioSource>();
                    if (!retryAudio.isPlaying) retryAudio.Play();

                    //Activates The Respawn Centre Object
                    respawnCentre.SetActive(true);
                }

                //Reference To The MoveToResetCentre Class
                moving = respawnCentre.GetComponent<MoveToResetCentre>();

                moving.walls = GameObject.FindGameObjectsWithTag("Wall");
                foreach (GameObject wall in moving.walls)
                {
                    wall.GetComponent<Renderer>().enabled = false;
                }

                moving.interactableObjects = GameObject.FindGameObjectsWithTag("InteractableObject");
                foreach (GameObject intObject in moving.interactableObjects)
                {
                    intObject.GetComponent<Renderer>().enabled = false;
                }

                if (orignUniverse != 0)
                {
                    this.gameObject.GetComponent<PlayerUniverseTracker>().currentUniverse = orignUniverse;
                }

            //Checks If The Object Is An Interactable Object, And If Is Has Not Fallen
            }
            else if (this.gameObject.tag == "InteractableObject")
            {
                if (hasObjectFallen == false)
                {
                    //References The Narrator's AudioSource Component
                    narratorAudio = GameObject.FindWithTag("Narrator").GetComponent<AudioSource>();

                    //References The Player's  Reset AudioSource Component
                    playerResetAudio = GameObject.FindWithTag("MainPlayerBody").GetComponent<AudioSource>();

                    //Checks If The Respawn Centre Object Is Not Active, And If The Narrator Is Not Currently Speaking
                    if (!narratorAudio.isPlaying && !playerResetAudio.isPlaying)
                    {
                        //Starts The Retry Audio Clip
                        AudioSource retryAudio = GetComponent<AudioSource>();
                        if (!retryAudio.isPlaying) retryAudio.Play();

                        //Shows That The Object Has Fallen Off The Level
                        hasObjectFallen = true;

                        if(usesPortals == true)
                        {
                            resetPositionRotation();
                        }
                    }
                }
            }
            else
            {
                resetPositionRotation();
            }
        }
        else
        {
            //Shows That The Object Has Not Fallen Off The Level
            hasObjectFallen = false;
        }
    }

    //This Function Transforms The Object Back To Its Spawn Point
    public void resetPositionRotation()
    {
        //Sets The Object's Position And Rotation To That Of The Spawn Point:
        //If It Is An Interactable Or Temporary Object
        if (this.gameObject.tag == "InteractableObject" || this.gameObject.tag == "TemporaryObject")
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

        //Otherwise The Object Is Sets It's First Recorded Position And Rotation
        }else{
            transform.position = spawnPosition;
            transform.rotation = Quaternion.Euler(spawnRotation);
        }

        //Keeps The Object In Place, Disables Its Gravity, And Enables Its Collider:
        //If The Object Is A Temporary Object
        if (this.gameObject.tag == "TemporaryObject")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);

            rb.useGravity = false;
            coll.enabled = true;
        }

        //Sets The Object's Current Universe To It's Original Universe:
        //If The Original Universe Number Is Not Set To Be 0
        if (orignUniverse != 0)
        {
            this.gameObject.GetComponent<ChangeObjectLayer>().currentUniverse = orignUniverse;
        }

        //References The Rigidbody Component
        rb = this.gameObject.GetComponent<Rigidbody>();

        //Sets The Velocity And Angular Velocity Of The Object To 0
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
