using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.VFX;

using Oculus.Interaction;

//This Class Allows The Past Player To Copy A Specific Attempt
public class CopyPlayer : MonoBehaviour
{
    
    //==================================================================PLAYER==================================================================//
    [Header("PLAYER")]

    //The Player's Body Parts
    public GameObject leftHand;
    public GameObject leftAnchor;
    public GameObject leftWrist;

    public GameObject rightHand;
    public GameObject rightAnchor;
    public GameObject rightWrist;

    public GameObject vrHead;

    //Reference To The RecordRotationPosition Class
    private GameObject playerControl;
    private RecordRotationPosition record;

    //Visual Models Of The Right And Left Hand
    public HandVisual rightHandVisual;
    public HandVisual leftHandVisual;

    public GameObject playerBody;
    public GameObject playerBodyJoints;

    //==================================================================ITERATIONS==================================================================//
    [Header("ITERATION")]

    //Iteration Value
    public int iteration = -1;

    //Stores The Iteration Value At The Start And End Of The Attempt
    private int attemptStart;
    private int attemptEnd;

    //Stores The Current Iteration Used
    private int index;

    //==================================================================INTERACTABLE OBJECTS==================================================================//
    [Header("INTERACTABLE OBJECTS")]

    //List Of All Interactable Objects
    private List<GameObject> interactableObjects;

    //Reference To The RecordInteractableObject Class
    private RecordInteractableObject recordObject;

    //Used As The RigidBody Of Each Interactable Object
    private Rigidbody rbObject;

    //==================================================================GRABBING==================================================================//
    [Header("GRABBING")]

    //Measure How Long Until The Object Grabbed Can Stop Follow The Right/Left Hand
    private int rightGrabWait;
    private int leftGrabWait;

    //Allows the Past Player To Interact With Objects
    public PastGrabObject grabObjectRight;
    public PastGrabObject grabObjectLeft;

    //==================================================================REWIND==================================================================//
    [Header("REWIND")]

    //Shows If The Past Player Is In Reverse
    private bool isInReverse;
    private bool prevIsInReverse;

    //The Rewind Audio
    private AudioSource rewindAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Initialises The Reference To The RecordRotationPosition Class
        playerControl = GameObject.FindWithTag("PlayerControl");
        record = playerControl.GetComponent<RecordRotationPosition>();

        //Stores A List Of All Interactable Objects In interactableObjects
        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

        //Sets The Grab Wait Of The Right And Left Hand To 0
        rightGrabWait = -1;
        leftGrabWait = 0;

        //Initialises The rewindAudio, And Stops The Audio
        rewindAudio = GetComponent<AudioSource>();
        if (rewindAudio.isPlaying) rewindAudio.Stop();

        //Shows That The Past Player Is Not In Reverse
        isInReverse = false;
    }

    //This Class Is Used To Check Which Iteration Should Be Used
    public void StartPastAttempt(int attemptIndex, int attemptPeriodStart, int attemptPeriodEnd)
    {
        //Initialises The Reference To The RecordRotationPosition Class
        playerControl = GameObject.FindWithTag("PlayerControl");
        record = playerControl.GetComponent<RecordRotationPosition>();

        //Stores Values For The Object Grabbed By The Left And Right Hand Into Their Respective Variables
        grabObjectLeft = leftHand.GetComponent<PastGrabObject>();
        grabObjectRight = rightHand.GetComponent<PastGrabObject>();

        //Stores The Start And End Of The Current Attempt, As Well As The Current Iteration, In Their Respective Variables
        attemptEnd = attemptPeriodEnd;
        attemptStart = attemptPeriodStart;
        index = attemptIndex;

        //If iteration Is Equal To -1, It Is Set To The Iteration At The Start Of The Attempt
        if (iteration == -1)
        {
            iteration = attemptPeriodStart;
        }
        else
        {

            //Checks If The Rewind Mechanic Is Active, And The Current Iteration Value Is Greater Or Equal To
            //Five More Than The Iteration Value At The Start Of The Attempt
            if (record.rewindTrigger == true && iteration >= 5 + attemptPeriodStart)
            {

                //Decrements iteration By One, And Shows That The Past Player Is In Reverse
                iteration--;
                isInReverse = true;

            //Checks If The Rewind Mechanic Is Not Active
            }
            else if(record.rewindTrigger == false)
            {

                //iteration Is Incremented By 1
                iteration++;

                //Checks If The Past Player Is In Reverse
                if(isInReverse == true)
                {
                    //Enables The Object's Gravity
                    rbObject.useGravity = true;
                    rbObject.isKinematic = false;

                    //The Velocity And Angular Velocity Of The Current Interactable Object Are Set To The Last Value Of Their Respective Lists
                    rbObject.velocity = recordObject.objectVelocity[recordObject.objectVelocity.Count - 1];
                    rbObject.angularVelocity = recordObject.objectAngularVelocity[recordObject.objectAngularVelocity.Count - 1];
                }

                //Shows That The Past Player Is Not In Reverse
                isInReverse = false;
            }
        }

        //Checks If The Current Iteration Is Less Or Equal To The Iteration Value At The End Of The Attempt
        if (iteration <= attemptPeriodEnd)
        {
            //If The Left Hand Is Not Grabbing An Object, Then It Is Set As Active
            if (record.leftGrab[iteration] == false)
            {
                leftHand.SetActive(true);
                leftAnchor.SetActive(true);

            //Otherwise The Left Hand Is Set As Not Active
            }else{
                leftHand.SetActive(false);
                leftAnchor.SetActive(false);
            }

            //If The Right Hand Is Not Grabbing An Object, Then It Is Set As Active
            if (record.rightGrab[iteration] == false)
            {
                rightHand.SetActive(true);
                rightAnchor.SetActive(true);

            //Otherwise The Right Hand Is Set As Not Active
            }
            else
            {
                rightHand.SetActive(false);
                rightAnchor.SetActive(false);
            }

            //The Past Player's Body Is Set As Active
            playerBody.SetActive(true);
            playerBodyJoints.SetActive(true);

        //Otherwise The Current Iteration Is Greater Than The Iteration Value At The End Of The Attempt
        }
        else
        {
            //All Body Parts Are Set As Not Active
            leftHand.SetActive(false);
            leftAnchor.SetActive(false);

            rightHand.SetActive(false);
            rightAnchor.SetActive(false);

            playerBody.SetActive(false);
            playerBodyJoints.SetActive(false);

            //Shows That The Right Hand Is Not Grabbing
            rightGrabWait = -1;

            //Shows That The Left Hand Is Not Grabbing
            leftGrabWait = 0;
        }

        //Checks If The stopRewind List In The Class RecordRotationPosition Has a Value
        if (record.stopRewind != null)
        {
            //Goes Through Every Value In The stopRewind List
            foreach (int stop in record.stopRewind)
            {
                //If The Current Value Is Equal To The Current Iteration, And The Current Iteration Is Not Equal To 0, Then The rewindAudio Is Played
                if (stop == iteration && iteration != 0)
                {
                    rewindAudio = GetComponent<AudioSource>();
                    if (rewindAudio.isPlaying) rewindAudio.Stop();
                    if (!rewindAudio.isPlaying) rewindAudio.Play();
                }
            }
        }

        //Calls The Replay Function
        StartCoroutine(Replay());
        
    }

    //This Function Replays Everything Within A Specific Iteration
    IEnumerator Replay()
    {
        //Initialises The Reference To The RecordRotationPosition Class
        playerControl = GameObject.FindWithTag("PlayerControl");
        record = playerControl.GetComponent<RecordRotationPosition>();

        //Waits For A Specific Amount Of Time Equal To timePerRecording, Found In The RecordRotationPosition Class
        yield return new WaitForSeconds(record.timePerRecording);

        //Checks If The Current Iteration Is Less Or Equal To The Iteration Value At The End Of The Attempt
        if (iteration <= attemptEnd)
        {

            //The Past Player's Head Is Set To The Position, And Rotation Specified By The iteration Value And Their Respecitve Lists Found In The RecordRotationPosition Class
            vrHead.transform.position = record.headPosition[iteration];
            vrHead.transform.rotation = record.headRotation[iteration];

            //The Past Player's Head Is Tweaked To Put It In The Correct Rotation
            vrHead.transform.Rotate(new Vector3(0, 0, 0));

            for (int i = 0; i < rightHandVisual._jointTransforms.Count; i++)
            {
                //The Past Player's Right Hand Is Set To The Position, And Rotation Specified By The iteration Value And Their Respecitve Lists Found In The RecordRotationPosition Class
                rightHandVisual._jointTransforms[i].position = record.rightFingerBonePosition[iteration][i];
                rightHandVisual._jointTransforms[i].rotation = record.rightFingerBoneRotation[iteration][i];

                //The Past Player's Left Hand Is Set To The Position, And Rotation Specified By The iteration Value And Their Respecitve Lists Found In The RecordRotationPosition Class
                leftHandVisual._jointTransforms[i].position = record.leftFingerBonePosition[iteration][i];
                leftHandVisual._jointTransforms[i].rotation = record.leftFingerBoneRotation[iteration][i];
            }
            leftAnchor.transform.position = leftWrist.transform.position;
            leftAnchor.transform.rotation = leftWrist.transform.rotation;

            rightAnchor.transform.position = rightWrist.transform.position;
            rightAnchor.transform.rotation = rightWrist.transform.rotation;
        }

        //Goes Through Every Object In The interactableObjects List
        foreach (GameObject intObject in interactableObjects)
        {
            //Stores The Reference To The RecordInteractableObject Class, And The RigidBody For The Specific Object In Their Respective Variables
            recordObject = intObject.GetComponent<RecordInteractableObject>();
            rbObject = intObject.GetComponent<Rigidbody>();

            //Checks If The Past Player Is Not In Reverse
            if (isInReverse == false)
            {
                //Checks If The Object Is Currently Not Being Grabbed By The Player
                if (intObject.GetComponent<Grabbable>().isBeingGrabbed == false)
                {
                    //Checks If The Current Iteration Of The Right Hand Is Grabbing
                    if (record.rightGrab[iteration] == true)
                    {
                        //Checks If The Object Grabbed By The Right Hand Is The Current Object, And If iteration Is Less Than The Iteration Value At The End OF The Attempt - 5
                        if (grabObjectRight.grabbedObject == intObject && iteration < (attemptEnd - 5))
                        {
                            //Disables The Object's Gravity
                            rbObject.useGravity = false;
                            rbObject.isKinematic = true;

                            //The Object's Position And Rotation Are Set To That Of The Right Hand
                            intObject.transform.position = rightWrist.transform.position;
                            intObject.transform.rotation = recordObject.objectRotation[iteration];

                            //The Object's Velocity And Angular Velocity Are Set To 0
                            rbObject.velocity = new Vector3(0, 0, 0);
                            rbObject.angularVelocity = new Vector3(0, 0, 0);

                            rightGrabWait = 0;

                            //Disables The Object's Collider
                            intObject.GetComponent<Collider>().enabled = false;
                        }

                        //Checks If The Current Iteration Of The Right Hand Is Not Grabbing And If rightGrabWait Is Greater Or Equal To 5
                    }
                    else if (record.rightGrab[iteration] == false && rightGrabWait >= 5)
                    {
                        //rightGrabWait Is Set To -1
                        rightGrabWait = -1;

                        //Enables The Object's Collider
                        intObject.GetComponent<Collider>().enabled = true;
                        //Checks If The Current Iteration Of The Right Hand Is Not Grabbing And If rightGrabWait Is Equal To 0
                    }
                    else if (record.rightGrab[iteration] == false && rightGrabWait < 5 && rightGrabWait >= 0)
                    {
                        //Checks If The Current Iteration Is Less Than The Iteration Value At The End Of The Attempt - 5
                        if (iteration < (attemptEnd - 5))
                        {
                            //Enables The Object's Gravity
                            rbObject.useGravity = true;
                            rbObject.isKinematic = false;

                            //The Object's Position And Rotation Are Set To That Specified Using The iteration Value And Their Respective Lists From The RecordInteractableObject Class
                            intObject.transform.position = recordObject.objectPosition[iteration];
                            intObject.transform.rotation = recordObject.objectRotation[iteration];

                            //The Object's Velocity And Angular Velocity Are Set To That Specified Using The iteration Value And Their Respective Lists From The RecordInteractableObject Class
                            rbObject.velocity = recordObject.objectVelocity[iteration];
                            rbObject.angularVelocity = recordObject.objectAngularVelocity[iteration];

                            //Enables The Object's Collider
                            intObject.GetComponent<Collider>().enabled = true;
                        }

                        //The rightGrabWait Variable Is Incremented By 1
                        rightGrabWait++;

                    }

                    //Checks If The Current Iteration Of The Left Hand Is Grabbing
                    if (record.leftGrab[iteration] == true)
                    {
                        //Checks If The Object Grabbed By The Left Hand Is The Current Object, And If iteration Is Less Than The Iteration Value At The End OF The Attempt - 5
                        if (grabObjectLeft.grabbedObject == intObject && iteration < (attemptEnd - 5))
                        {
                            //Disables The Object's Gravity
                            rbObject.useGravity = false;
                            rbObject.isKinematic = true;

                            //The Object's Position And Rotation Are Set To That Of The Left Hand
                            intObject.transform.position = leftWrist.transform.position;
                            intObject.transform.rotation = recordObject.objectRotation[iteration];

                            //The Object's Velocity And Angular Velocity Are Set To 0
                            rbObject.velocity = new Vector3(0, 0, 0);
                            rbObject.angularVelocity = new Vector3(0, 0, 0);

                            leftGrabWait = 0;

                            //Disables The Object's Collider
                            intObject.GetComponent<Collider>().enabled = false;
                        }

                        //Checks If The Current Iteration Of The Left Hand Is Not Grabbing And If leftGrabWait Is Greater Or Equal To 5
                    }
                    else if (record.leftGrab[iteration] == false && leftGrabWait >= 5)
                    {
                        //leftGrabWait Is Set To -1
                        leftGrabWait = -1;

                        //Enables The Object's Collider
                        intObject.GetComponent<Collider>().enabled = true;

                        //Checks If The Current Iteration Of The Left Hand Is Not Grabbing And If leftGrabWait Is Equal To 0
                    }
                    else if (record.leftGrab[iteration] == false && leftGrabWait < 5 && leftGrabWait >= 0)
                    {
                        //Checks If The Current Iteration Is Less Than The Iteration Value At The End Of The Attempt - 5
                        if (iteration < (attemptEnd - 5))
                        {
                            //Enables The Object's Gravity
                            rbObject.useGravity = true;
                            rbObject.isKinematic = false;

                            //The Object's Position And Rotation Are Set To That Specified Using The iteration Value And Their Respective Lists From The RecordInteractableObject Class
                            intObject.transform.position = recordObject.objectPosition[iteration];
                            intObject.transform.rotation = recordObject.objectRotation[iteration];

                            //The Object's Velocity And Angular Velocity Are Set To That Specified Using The iteration Value And Their Respective Lists From The RecordInteractableObject Class
                            rbObject.velocity = recordObject.objectVelocity[iteration];
                            rbObject.angularVelocity = recordObject.objectAngularVelocity[iteration];
                        }

                        //The leftGrabWait Variable Is Incremented By 1
                        leftGrabWait++;

                        //Enables The Object's Collider
                        intObject.GetComponent<Collider>().enabled = true;
                    }

                }
                else
                {
                    //Prevents The Object From Being Moved If It Is Grabbed By The Player
                    rightGrabWait = -1;
                    leftGrabWait = -1;
                }
            }

            //Checks If The Past Player Is In Reverse
            if (isInReverse == true)
            {
                //Disables The Object's Gravity
                rbObject.useGravity = false;
                rbObject.isKinematic = true;

                //Enables The Object's Collider
                intObject.GetComponent<Collider>().enabled = true;

                //Checks If The Right Hand Is Grabbing An Object
                if (grabObjectRight.grabbedObject == intObject && record.rightGrab[iteration] == true)
                {
                    //The Object's Position And Rotation Are Set To That Of The Right Hand
                    intObject.transform.position = recordObject.objectPosition[iteration];
                    intObject.transform.rotation = recordObject.objectRotation[iteration];

                    //The Object's Velocity And Angular Velocity Are Set To 0 
                    rbObject.velocity = new Vector3(0, 0, 0);
                    rbObject.angularVelocity = new Vector3(0, 0, 0);
                }
                else if(grabObjectLeft.grabbedObject == intObject && record.leftGrab[iteration] == true)
                {
                    //The Object's Position And Rotation Are Set To That Of The Left Hand
                    intObject.transform.position = recordObject.objectPosition[iteration];
                    intObject.transform.rotation = recordObject.objectRotation[iteration];

                    //The Object's Velocity And Angular Velocity Are Set To 0 
                    rbObject.velocity = new Vector3(0, 0, 0);
                    rbObject.angularVelocity = new Vector3(0, 0, 0);
                }

                //rightGrabWait Is Set To -1
                rightGrabWait = -1;

                //leftGrabWait Is Set To -1
                leftGrabWait = -1;
            }
        }

        //The StartPastAttempt Function Is Called, Using The Same Inputs For The Function
        StartPastAttempt(index, attemptStart, attemptEnd);
    }
}
