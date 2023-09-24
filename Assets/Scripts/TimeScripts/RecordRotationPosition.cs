using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.GrabAPI;

//This Class Is Used To Record Data About The Player, As Well As Interactable And Temporary Objects
public class RecordRotationPosition : MonoBehaviour
{
    //Background Music
    public BackGroundMusicControl backGround;

    //List of All The Objects And The Player
    private List<GameObject> allObjects;
    //==================================================================RESET==================================================================//
    [Header("RESET")]

    [HideInInspector]
    //Shows If The Level Is Resetting
    public bool resetPosition = false;

    [HideInInspector]
    //Shows If The Reset Trigger Of Either Button Is Activated
    public bool resetTrigger;

    //==================================================================REWIND==================================================================//
    [Header("REWIND")]
    [HideInInspector]
    //Shows If The Rewind Mechanic Is Activated
    public bool rewindTrigger;

    //==================================================================PLAYER==================================================================//
    [Header("PLAYER")]
    //Player Body Parts
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject VRHead;

    [HideInInspector]
    //Time Between Each Record
    public float timePerRecording;

    [HideInInspector]
    //List of all attempts
    public List <int> attempts = new List <int>();

    //==============================PLAYER RIGHT HAND==============================//
    [Header("PLAYER RIGHT HAND")]

    //Recording Lists For The Rotation, Position, Velocity, Angular Velocity, And Grabbing Of The Right Hand
    [HideInInspector]
    public List<List<Vector3>> rightFingerBonePosition = new List<List<Vector3>>();
    [HideInInspector]
    public List<List<Quaternion>> rightFingerBoneRotation = new List<List<Quaternion>>();

    [HideInInspector]
    public List<List<Vector3>> leftFingerBonePosition = new List<List<Vector3>>();
    [HideInInspector]
    public List<List<Quaternion>> leftFingerBoneRotation = new List<List<Quaternion>>();

    [HideInInspector]
    public List<Vector3> rightVelocity = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> rightAngularVelocity = new List<Vector3>();

    [HideInInspector]
    public List<bool> rightGrab = new List<bool>();

    [HideInInspector]
    //Stores The Current Velocity Of the Right Hand
    public Vector3 rightHandVelocity;

    [HideInInspector]
    //Stores The Current Angular Velocity Of the Right Hand
    public Vector3 rightHandAngularVelocity;

    //Visual Model Of The Right Hand
    public HandVisual rightHandVisual;

    //Last Recorded Position And Rotation Of The Right Hand
    private Vector3 prevRightHandPos;
    private Vector3 prevRightHandRot;

    //Reference To The HandGrabAPI Class Of the Right Hand
    public HandGrabAPI rightHandGrabAPI;

    //==============================PLAYER LEFT HAND==============================//
    [Header("PLAYER LEFT HAND")]

    //Recording Lists For The Rotation, Position, Velocity, Angular Velocity, And Grabbing Of The Left Hand
    [HideInInspector]
    public List<Quaternion> leftRotation = new List<Quaternion>();
    [HideInInspector]
    public List<Vector3> leftPosition = new List<Vector3>();

    [HideInInspector]
    public List<Vector3> leftVelocity = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> leftAngularVelocity = new List<Vector3>();

    [HideInInspector]
    public List<bool> leftGrab = new List<bool>();

    [HideInInspector]
    //Stores The Current Velocity Of the Left Hand
    public Vector3 leftHandVelocity;

    [HideInInspector]
    //Stores The Current Angular Velocity Of the Left Hand
    public Vector3 leftHandAngularVelocity;

    //Visual Model Of The Right Hand
    public HandVisual leftHandVisual;

    //Last Recorded Position And Rotation Of The Left Hand
    private Vector3 prevLeftHandPos;
    private Vector3 prevLeftHandRot;

    //Reference To The HandGrabAPI Class Of the Left Hand
    public HandGrabAPI leftHandGrabAPI;

    //==============================PLAYER HEAD==============================//
    [Header("PLAYER HEAD")]

    //Recording Lists For The Rotation, And Position Of The Head
    [HideInInspector]
    public List<Quaternion> headRotation = new List<Quaternion>();
    [HideInInspector]
    public List<Vector3> headPosition = new List<Vector3>();

    //==================================================================PAST PLAYER==================================================================//
    [Header("PAST PLAYER")]

    //Past Player Prefab
    public GameObject pastPlayer;

    //Array Of Created Past Players
    private GameObject[] pastPlayers;

    //==================================================================REWIND PLAYER==================================================================//
    [Header("REWIND PLAYER")]

    //Rewind Player Prefab
    public GameObject rewindPlayer;

    //Player Rewind Audio
    private AudioSource rewindAudio;

    [HideInInspector]
    //Recording List For When The 
    public List<int> stopRewind = new List<int>();

    [HideInInspector]
    public bool showRewind;

    private bool prevRewind;

    //============================================================INTERACTABLE OBJECTS============================================================//
    [Header("INTERACTABLE OBJECTS")]

    [HideInInspector]
    //List For All Interactable Objects
    public List<GameObject> interactableObjects;

    //Specific Components For Each Interactable Object
    private RecordInteractableObject recIntObject;
    private HandGrabInteractable recIntHandGrab;
    private Rigidbody rbIntObject;
    private Grabbable grabbableIntObject;

    //Stores The Average Velocity Of The Object
    private Vector3 meanVelocity;

    //Stores The Average Angular Velocity Of The Object
    private Vector3 meanAngularVelocity;

    //============================================================TEMPORARY OBJECTS============================================================//
    [Header("TEMPORARY OBJECTS")]

    //List For All Temporary Objects
    private List<GameObject> temporaryObjects;

    //Specific Components For Each Temporary Object
    private RecordTemporaryObject recTempObject;
    private Rigidbody rbTempObject;
    private Collider collTempObject;

    private AddExistingCorners add;

    [Header("HAND GRABBING")]
    public GameObject rightHandGrabber;
    public GameObject leftHandGrabber;

    void Start()
    {
        //Reference To The AddExistingCorners Class
        add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Stores The Objects In Their Respective Lists
        allObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

        temporaryObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("TemporaryObject"));

        
        //Adds The Temporary Objects To The allObjects List If There Are Any In The Level
        if(temporaryObjects != null)
        {
            allObjects.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("TemporaryObject")));
        }

        //Adds The Player To The allObjects List
        allObjects.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("MainPlayerBody")));

        //Starts The Level By Clarifying That It Is Not Rewinding
        showRewind = false;

        //Initialising And Setting The RewindAudio As Not Playing
        rewindAudio = GetComponent<AudioSource>();
        if (rewindAudio.isPlaying) rewindAudio.Stop();

        prevRightHandPos = Vector3.zero;
        prevRightHandRot = Vector3.zero;

        prevLeftHandPos = Vector3.zero;
        prevLeftHandRot = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timePerRecording = Time.deltaTime * 0.9f;

        //Checks If The Primary Or Secondary Button On The Left Controller Has Been Pressed
        if (resetTrigger == true)
        {
            //Shows That The Positions Of Everything In The Level Will Be Reset To Their Orignal Positions
            resetPosition = true;

            //Stores All Past Players In The Array
            pastPlayers = GameObject.FindGameObjectsWithTag("PastPlayer");

            //If There Are Any Interactable Objects, Then The HandGrabInteractable Component Of Each One Is Disabled
            if (interactableObjects.Count > 0)
            {
                foreach (GameObject intObject in interactableObjects)
                {
                    intObject.GetComponent<HandGrabInteractable>().enabled = false;
                    intObject.GetComponent<objectGravity>().objectReleased();
                }
            }          

            //Destroys All Existing Past Players
            for (var i = 0; i < pastPlayers.Length; i++)
            {
                Destroy(pastPlayers[i]);
            }

            //Resets The Position Of Every Object In The List allObject
            if (allObjects.Count > 0)
            {
                foreach (GameObject allObject in allObjects)
                {
                    allObject.GetComponent<ResetPosition>().resetPositionRotation();
                }
            }

            //Resets The Position Of All The Existing Temporary Objects
            if (temporaryObjects.Count > 0)
            {
                foreach (GameObject tempObject in temporaryObjects)
                {
                    tempObject.GetComponent<ResetPosition>().resetPositionRotation();
                }
            }

            //Creates A New Attempt, Storing The List Length Of All The Recording Lists
            attempts.Add(headPosition.Count);

            //Creates A Past Player For Each Attempt, And Gives Each One The Section Of The Recording Lists They Need To Replay
            for (int i = 0; i < attempts.Count; i++)
            {
                GameObject pastInstance = Instantiate(pastPlayer, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                CopyPlayer copy = pastInstance.GetComponent<CopyPlayer>();

                if(i == 0)
                {
                    copy.StartPastAttempt(i, 0, attempts[i]);
                }
                else
                {
                    copy.StartPastAttempt(i, attempts[i - 1] + 1, attempts[i]);
                } 
            }

            //Resets The Background Music
            backGround.ResetMusic();

            //Renabled the HandGrabInteractable Component Of All Interactable Objects
            if (interactableObjects.Count > 0)
            {
                foreach (GameObject intObject in interactableObjects)
                {
                    intObject.GetComponent<HandGrabInteractable>().enabled = true;
                }
            }

        //Checks If Both Reset Buttons Were Not Pressed
        }
        else if(resetTrigger == false)
        {

            //Shows That The Objects Will Not Reset To Their Orignal Positions
            resetPosition = false;
        }

        //Makes The Player Stop Grabbing During Rewind
        if(rewindTrigger == true)
        {
            rightHandGrabber.SetActive(false);
            leftHandGrabber.SetActive(false);
        }
        else
        {
            rightHandGrabber.SetActive(true);
            leftHandGrabber.SetActive(true);
        }

        //Checks If This Is Not The First Attempt
        if (attempts.Count > 0)
        {

            //Checks If The Rewind Mechanic Is Being Used, And If The Max Iteration Value Is Greater Or Equal To The Value Of The Last Attempt + 5
            if (rewindTrigger == true && headPosition.Count >= (attempts[attempts.Count - 1] + 5))
            {
                //If There Are Interactable Objects, The Objects Velocity And Angular Velocity Will Be Set To The Last Recorded Values In Their objectVelocity, And objectAngularVelocity Lists
                if (interactableObjects.Count > 0)
                {
                    foreach (GameObject intObject in interactableObjects)
                    {
                        recIntObject = intObject.GetComponent<RecordInteractableObject>();
                        rbIntObject = intObject.GetComponent<Rigidbody>();
                        rbIntObject.velocity = recIntObject.objectVelocity[recIntObject.objectVelocity.Count - 1];
                        rbIntObject.angularVelocity = recIntObject.objectAngularVelocity[recIntObject.objectAngularVelocity.Count - 1];
                    }
                }

                //If There Are Temporary Objects, The Objects Velocity And Angular Velocity Will Be Set To The Last Recorded Values In Their objectVelocity, And objectAngularVelocity Lists
                if (temporaryObjects.Count > 0)
                {
                    foreach (GameObject tempObject in temporaryObjects)
                    {
                        recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                        rbTempObject = tempObject.GetComponent<Rigidbody>();
                        rbTempObject.velocity = recTempObject.objectVelocity[recTempObject.objectVelocity.Count - 1];
                        rbTempObject.angularVelocity = recTempObject.objectAngularVelocity[recTempObject.objectAngularVelocity.Count - 1];
                    }
                }

                //Sets The Background Music To Play In Reverse
                backGround.RewindMusic();

                //Calls The ReWrite Function Used For The Rewind Mechanic
                StartCoroutine(ReWrite());

            //Checks If The Rewind Mechanic Is Being Used, And If The Max Iteration Value Is Less Or Equal To The Value Of The Last Attempt + 5
            }
            else if (rewindTrigger == true && headPosition.Count <= (attempts[attempts.Count - 1] + 5))
            {
                //If There Are Interactable Objects, The Objects Velocity And Angular Velocity Will Be Set To 0
                if (interactableObjects.Count > 0)
                {
                    foreach (GameObject intObject in interactableObjects)
                    {
                        recIntObject = intObject.GetComponent<RecordInteractableObject>();
                        rbIntObject = intObject.GetComponent<Rigidbody>();
                        rbIntObject.velocity = new Vector3(0, 0, 0);
                        rbIntObject.angularVelocity = new Vector3(0, 0, 0);
                    }
                }

                //If There Are Temporary Objects, The Objects Velocity And Angular Velocity Will Be Set To 0
                if (temporaryObjects.Count > 0)
                {
                    foreach (GameObject tempObject in temporaryObjects)
                    {
                        recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                        rbTempObject = tempObject.GetComponent<Rigidbody>();
                        rbTempObject.velocity = new Vector3(0, 0, 0);
                        rbTempObject.angularVelocity = new Vector3(0, 0, 0);
                    }
                }

                //Stops The Background Playing
                backGround.StopMusic();

                //Checks If The Greatest Amount Of Iterations Is Larger Or Equal To 0
                if(headPosition.Count >= 0) 
                {
                    //Calls The StandStill Function Used For The Rewind Mechanic When The Current Iteration Value Is Close To The Start Of The Current Attempt
                    StartCoroutine(StandStill());
                }


            //Checks If The Rewind Mechanic Is Not Being Used
            }
            else if (rewindTrigger == false)
            {
                //Checks If The Replay Player Is Being Shown
                if (showRewind == true)
                {
                    //Plays The Rewind Audio
                    if (rewindAudio.isPlaying) rewindAudio.Stop();
                    if (!rewindAudio.isPlaying) rewindAudio.Play();

                    //Add The Current Iteration Value To The stopRewind List
                    stopRewind.Add(headPosition.Count);

                    //Shows That The Replay Player Will No Longer Be Shown
                    showRewind = false;
                }

                //The Background Music Is Played As Normal
                backGround.ForwardMusic();

                //Calls The Record Function Used For Recording The Player And Objects
                StartCoroutine(Record());
            }

        //If This Is The First Attempt
        }
        else
        {
            //Checks If The Rewind Mechanic Is Being Used, And If The Max Iteration Value Is Greater Or Equal To 5
            if (rewindTrigger == true && headPosition.Count >= 5)
            {
                //If There Are Interactable Objects, The Objects Velocity And Angular Velocity Will Be Set To The Last Recorded Values In Their objectVelocity, And objectAngularVelocity Lists
                if (interactableObjects.Count > 0)
                {
                    foreach (GameObject intObject in interactableObjects)
                    {
                        recIntObject = intObject.GetComponent<RecordInteractableObject>();
                        rbIntObject = intObject.GetComponent<Rigidbody>();
                        rbIntObject.velocity = recIntObject.objectVelocity[recIntObject.objectVelocity.Count - 1];
                        rbIntObject.angularVelocity = recIntObject.objectAngularVelocity[recIntObject.objectAngularVelocity.Count - 1];
                    }
                }

                //If There Are Temporary Objects, The Objects Velocity And Angular Velocity Will Be Set To The Last Recorded Values In Their objectVelocity, And objectAngularVelocity Lists
                if (temporaryObjects.Count > 0)
                {
                    foreach (GameObject tempObject in temporaryObjects)
                    {
                        recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                        rbTempObject = tempObject.GetComponent<Rigidbody>();
                        rbTempObject.velocity = recTempObject.objectVelocity[recTempObject.objectVelocity.Count - 1];
                        rbTempObject.angularVelocity = recTempObject.objectAngularVelocity[recTempObject.objectAngularVelocity.Count - 1];
                    }
                }

                //Sets The Background Music To Play In Reverse
                backGround.RewindMusic();

                //Calls The ReWrite Function Used For The Rewind Mechanic
                StartCoroutine(ReWrite());

            //Checks If The Rewind Mechanic Is Being Used, And If The Max Iteration Value Is Less Than 5
            }
            else if(rewindTrigger == true && headPosition.Count < 5)
            {
                //If There Are Interactable Objects, The Objects Velocity And Angular Velocity Will Be Set To 0
                if (interactableObjects.Count > 0)
                {
                    foreach (GameObject intObject in interactableObjects)
                    {
                        recIntObject = intObject.GetComponent<RecordInteractableObject>();
                        rbIntObject = intObject.GetComponent<Rigidbody>();
                        rbIntObject.velocity = new Vector3(0, 0, 0);
                        rbIntObject.angularVelocity = new Vector3(0, 0, 0);
                    }
                }

                //If There Are Temporary Objects, The Objects Velocity And Angular Velocity Will Be Set To 0
                if (temporaryObjects.Count > 0)
                {
                    foreach (GameObject tempObject in temporaryObjects)
                    {
                        recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                        rbTempObject = tempObject.GetComponent<Rigidbody>();
                        rbTempObject.velocity = new Vector3(0, 0, 0);
                        rbTempObject.angularVelocity = new Vector3(0, 0, 0);
                    }
                }

                //Stops The Background Playing
                backGround.StopMusic();

                //Calls The StandStill Function Used For The Rewind Mechanic When The Current Iteration Value Is Close To The Start Of The Current Attempt
                StartCoroutine(StandStill());

            //Checks If Both Rewind Buttons Have Not Been Pressed
            }
            else if(rewindTrigger == false)
            {

                //Checks If The Replay Player Is Being Shown
                if (showRewind == true)
                {
                    //Plays The Rewind Audio
                    if (rewindAudio.isPlaying) rewindAudio.Stop();
                    if (!rewindAudio.isPlaying) rewindAudio.Play();

                    //Add The Current Iteration Value To The stopRewind List
                    stopRewind.Add(headPosition.Count);

                    //Shows That The Replay Player Will No Longer Be Shown
                    showRewind = false;
                }

                //The Background Music Is Played As Normal
                backGround.ForwardMusic();

                //Calls The Record Function Used For Recording The Player And Objects
                StartCoroutine(Record());
            }
        }
    }

    //This Function Is Used To Record Data About The Player And Objects
    IEnumerator Record()
    {
        //Waits For A Specified Amount Of Time Before Recording
        yield return new WaitForSeconds(timePerRecording);
        if (add.isResetActivated == true)
        {
            pastPlayers = GameObject.FindGameObjectsWithTag("PastPlayer");

            //Destroys All Existing Past Players
            for (var i = 0; i < pastPlayers.Length; i++)
            {
                pastPlayers[i].SetActive(false);
            }

            //If There Are Interactable Objects, Velocity, Angular Velocity And Gravity Of Each Object Are Reset
            if (interactableObjects.Count > 0)
            {
                foreach (GameObject intObject in interactableObjects)
                {
                    rbIntObject = intObject.GetComponent<Rigidbody>();

                    rbIntObject.useGravity = false;
                    rbIntObject.isKinematic = true;

                    rbIntObject.velocity = new Vector3(0, 0, 0);
                    rbIntObject.angularVelocity = new Vector3(0, 0, 0);
                }
            }

            //If There Are Temporary Objects, The Velocity, Angular Velocity And Gravity Of Each Object Are Reset
            if (temporaryObjects.Count > 0)
            {
                foreach (GameObject tempObject in temporaryObjects)
                {
                    rbTempObject = tempObject.GetComponent<Rigidbody>();

                    rbTempObject.velocity = new Vector3(0, 0, 0);
                    rbTempObject.angularVelocity = new Vector3(0, 0, 0);
                    rbTempObject.useGravity = false;
                }
            }
        }
        else if (add.isResetActivated == false)
        {
            //Stores The Position And Rotation About The Head, Left Hand, And Right Hand In Their Respective Lists
            headPosition.Add(VRHead.transform.position);
            headRotation.Add(VRHead.transform.rotation);

            //Stores The Position And Rotation Of Each Bone For The Left And Right Hand
            rightFingerBonePosition.Add(new List<Vector3>());
            rightFingerBoneRotation.Add(new List<Quaternion>());

            leftFingerBonePosition.Add(new List<Vector3>());
            leftFingerBoneRotation.Add(new List<Quaternion>());

            for (int i = 0; i < rightHandVisual._jointTransforms.Count; i++)
            {
                rightFingerBonePosition[rightFingerBonePosition.Count - 1].Add(rightHandVisual._jointTransforms[i].position);
                rightFingerBoneRotation[rightFingerBoneRotation.Count - 1].Add(rightHandVisual._jointTransforms[i].rotation);

                leftFingerBonePosition[leftFingerBonePosition.Count - 1].Add(leftHandVisual._jointTransforms[i].position);
                leftFingerBoneRotation[leftFingerBoneRotation.Count - 1].Add(leftHandVisual._jointTransforms[i].rotation);
            }

            //Stores If The Left And Right Hands Are Currently Grabbing In Their Respective Lists
            rightGrab.Add(rightHandGrabAPI.publicAnyHolding);

            leftGrab.Add(leftHandGrabAPI.publicAnyHolding);

            //Stores The Velocity And Angular Velocity Of The Right And Left Hand In Their Respective Lists
            rightHandVelocity = (rightHand.transform.position - prevRightHandPos) / timePerRecording;
            prevRightHandPos = rightHand.transform.position;

            rightHandAngularVelocity = (rightHand.transform.rotation.eulerAngles - prevRightHandRot) / timePerRecording;
            prevRightHandRot = rightHand.transform.rotation.eulerAngles;

            rightVelocity.Add(rightHandVelocity);
            rightAngularVelocity.Add(rightHandAngularVelocity);

            leftHandVelocity = (leftHand.transform.position - prevLeftHandPos) / timePerRecording;
            prevLeftHandPos = leftHand.transform.position;

            leftHandAngularVelocity = (leftHand.transform.rotation.eulerAngles - prevLeftHandRot) / timePerRecording;
            prevLeftHandRot = leftHand.transform.rotation.eulerAngles;

            leftVelocity.Add(leftHandVelocity);
            leftAngularVelocity.Add(leftHandAngularVelocity);

            //If There Are Interactable Objects, The Position, Rotation, Velocity, And Angular Velocity Of Each Object Is Stored In Their Respective Lists
            if (interactableObjects.Count > 0)
            {
                foreach (GameObject intObject in interactableObjects)
                {
                    recIntObject = intObject.GetComponent<RecordInteractableObject>();
                    rbIntObject = intObject.GetComponent<Rigidbody>();
                    grabbableIntObject = intObject.GetComponent<Grabbable>();

                    //The HandGrabInterable Component Is Activated To Allow The Player To Hold The Object
                    recIntHandGrab = intObject.GetComponent<HandGrabInteractable>();
                    recIntHandGrab.enabled = true;

                    recIntObject.objectPosition.Add(intObject.transform.position);
                    recIntObject.objectRotation.Add(intObject.transform.rotation);

                    recIntObject.objectVelocity.Add(rbIntObject.velocity);
                    recIntObject.objectAngularVelocity.Add(rbIntObject.angularVelocity);
                }
            }

            //If There Are Temporary Objects, The Position, Rotation, Velocity, Angular Velocity, Gravity, And Collision Of Each Object Is Stored In Their Respective Lists
            if (temporaryObjects.Count > 0)
            {
                foreach (GameObject tempObject in temporaryObjects)
                {
                    recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                    rbTempObject = tempObject.GetComponent<Rigidbody>();
                    collTempObject = tempObject.GetComponent<Collider>();

                    recTempObject.objectPosition.Add(tempObject.transform.position);
                    recTempObject.objectRotation.Add(tempObject.transform.rotation);

                    recTempObject.objectVelocity.Add(rbTempObject.velocity);
                    recTempObject.objectAngularVelocity.Add(rbTempObject.angularVelocity);

                    recTempObject.objectGravity.Add(rbTempObject.useGravity);
                    recTempObject.objectCollision.Add(collTempObject.enabled);
                }
            }
        }
    }

    //This Function Is Used To Rewrite Data When Rewinding And Undoing Certain Actions
    IEnumerator ReWrite()
    {
        //Waits For A Specified Amount Of Time Before Recording
        yield return new WaitForSeconds(timePerRecording);

        //Checks If The Rewind Player Is Not Shown
        if (showRewind == false)
        {
            //It States That The Rewind Player Will Be Shown, And Creates An Instance Of A Rewind Player
            showRewind = true;
            GameObject rewindInstance = Instantiate(rewindPlayer, transform.position, Quaternion.identity) as GameObject;
        }

        //Removes The Latest Data Of The Position And Rotation About The Head, Left Hand, And Right Hand From Their Respective Lists
        headPosition.RemoveAt(headPosition.Count - 1);
        headRotation.RemoveAt(headRotation.Count - 1);

        rightFingerBonePosition.RemoveAt(rightFingerBonePosition.Count - 1);
        rightFingerBoneRotation.RemoveAt(rightFingerBoneRotation.Count - 1);

        leftFingerBonePosition.RemoveAt(leftFingerBonePosition.Count - 1);
        leftFingerBoneRotation.RemoveAt(leftFingerBoneRotation.Count - 1);

        //Removes The Latest Data Of The Trigger Value Of The Right And Left Controller From Their Respective Lists
        rightGrab.RemoveAt(rightGrab.Count - 1);
        leftGrab.RemoveAt(leftGrab.Count - 1);

        //Removes The Latest Data Of The Velocity And Angular Velocity Of The Right And Left Controller From Their Respective Lists
        rightVelocity.RemoveAt(rightVelocity.Count - 1);
        rightAngularVelocity.RemoveAt(rightAngularVelocity.Count - 1);

        leftVelocity.RemoveAt(leftVelocity.Count - 1);
        leftAngularVelocity.RemoveAt(leftAngularVelocity.Count - 1);

        //If There Are Interactable Objects, The Position, Rotation, Velocity, And Angular Velocity Of Each Object Are Set To The Latest Values From Their Respective Lists, Which Are Then Removed From The Lists
        if (interactableObjects.Count > 0)
        {
            foreach (GameObject intObject in interactableObjects)
            {
                recIntObject = intObject.GetComponent<RecordInteractableObject>();
                rbIntObject = intObject.GetComponent<Rigidbody>();

                //The HandGrabInterable Component Is Deactivated To Prevent The Player From Holding Them
                recIntHandGrab = intObject.GetComponent<HandGrabInteractable>();
                recIntHandGrab.enabled = false;

                pastPlayers = GameObject.FindGameObjectsWithTag("PastPlayer");

                //Checks If There Are Any Past Players In The Level
                if(pastPlayers.Length > 0)
                {
                    //Sets The Position and Rotation Of The Object To  Set To The Latest Values Recorded If No Past Player Is Grabbing
                    foreach (GameObject past in pastPlayers)
                    {
                        CopyPlayer copyPlayer = past.GetComponent<CopyPlayer>();

                        if (rightGrab[copyPlayer.iteration] == false && leftGrab[copyPlayer.iteration] == false)
                        {
                            intObject.transform.position = recIntObject.objectPosition[recIntObject.objectPosition.Count - 1];
                            intObject.transform.rotation = recIntObject.objectRotation[recIntObject.objectRotation.Count - 1];
                        }
                    }
                }
                else
                {
                    //Sets The Position and Rotation Of The Object To  Set To The Latest Values Recorded
                    intObject.transform.position = recIntObject.objectPosition[recIntObject.objectPosition.Count - 1];
                    intObject.transform.rotation = recIntObject.objectRotation[recIntObject.objectRotation.Count - 1];
                }
                            

                rbIntObject.velocity = recIntObject.objectVelocity[recIntObject.objectVelocity.Count - 1];
                rbIntObject.angularVelocity = recIntObject.objectAngularVelocity[recIntObject.objectAngularVelocity.Count - 1];

                recIntObject.objectPosition.RemoveAt(recIntObject.objectPosition.Count - 1);
                recIntObject.objectRotation.RemoveAt(recIntObject.objectRotation.Count - 1);

                recIntObject.objectVelocity.RemoveAt(recIntObject.objectVelocity.Count - 1);
                recIntObject.objectAngularVelocity.RemoveAt(recIntObject.objectAngularVelocity.Count - 1);
            }
        }

        //If There Are Temporary Objects, The Position, Rotation, Gravity, Collision, Velocity, And Angular Velocity Of Each Object Are Set To The Latest Values From Their Respective Lists, Which Are Then Removed From The Lists
        if (temporaryObjects.Count > 0)
        {
            foreach (GameObject tempObject in temporaryObjects)
            {
                recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                rbTempObject = tempObject.GetComponent<Rigidbody>();
                collTempObject = tempObject.GetComponent<Collider>();

                tempObject.transform.position = recTempObject.objectPosition[recTempObject.objectPosition.Count - 1];
                tempObject.transform.rotation = recTempObject.objectRotation[recTempObject.objectRotation.Count - 1];

                rbTempObject.useGravity = recTempObject.objectGravity[recTempObject.objectGravity.Count - 1];
                collTempObject.enabled = recTempObject.objectCollision[recTempObject.objectCollision.Count - 1];

                recTempObject.objectPosition.RemoveAt(recTempObject.objectPosition.Count - 1);
                recTempObject.objectRotation.RemoveAt(recTempObject.objectRotation.Count - 1);

                recTempObject.objectVelocity.RemoveAt(recTempObject.objectVelocity.Count - 1);
                recTempObject.objectAngularVelocity.RemoveAt(recTempObject.objectAngularVelocity.Count - 1);

                recTempObject.objectGravity.RemoveAt(recTempObject.objectGravity.Count - 1);
                recTempObject.objectCollision.RemoveAt(recTempObject.objectCollision.Count - 1);
            }
        }
    }

    //This Function Is Used When The Player Rewinds To When The Level Started
    IEnumerator StandStill()
    {
        //Waits For A Specified Amount Of Time Before Recording
        yield return new WaitForSeconds(timePerRecording);

        //Checks If The Rewind Player Is Not Shown
        if (showRewind == false)
        {
            //It States That The Rewind Player Will Be Shown, And Creates An Instance Of A Rewind Player
            showRewind = true;
            GameObject rewindInstance = Instantiate(rewindPlayer, transform.position, Quaternion.identity) as GameObject;
        }

        //If There Are Interactable Objects, The Position, Rotation, Velocity, And Angular Velocity Of Each Object Are Set To 0
        if (interactableObjects.Count > 0)
        {
            foreach (GameObject intObject in interactableObjects)
            {
                recIntObject = intObject.GetComponent<RecordInteractableObject>();
                rbIntObject = intObject.GetComponent<Rigidbody>();

                //The HandGrabInterable Component Is Deactivated To Prevent The Player From Holding Them
                recIntHandGrab = intObject.GetComponent<HandGrabInteractable>();
                recIntHandGrab.enabled = false;

                intObject.transform.position = recIntObject.objectPosition[0];
                intObject.transform.rotation = recIntObject.objectRotation[0];

                rbIntObject.velocity = new Vector3(0, 0, 0);
                rbIntObject.angularVelocity = new Vector3(0, 0, 0);
            }
        }

        //If There Are Temporary Objects, The Position, Rotation, Gravity, Collision, Velocity, And Angular Velocity Of Each Object Are Set To 0
        if (temporaryObjects.Count > 0)
        {
            foreach (GameObject tempObject in temporaryObjects)
            {
                recTempObject = tempObject.GetComponent<RecordTemporaryObject>();
                rbTempObject = tempObject.GetComponent<Rigidbody>();
                collTempObject = tempObject.GetComponent<Collider>();

                tempObject.transform.position = recTempObject.objectPosition[0];
                tempObject.transform.rotation = recTempObject.objectRotation[0];

                rbTempObject.useGravity = recTempObject.objectGravity[0];
                collTempObject.enabled = recTempObject.objectCollision[0];

                rbTempObject.velocity = new Vector3(0, 0, 0);
                rbTempObject.angularVelocity = new Vector3(0, 0, 0);  
            }
        }
    }
}
