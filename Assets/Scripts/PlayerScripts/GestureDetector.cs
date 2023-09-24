//This Was Adapted From:
//Valem, Hand Tracking Gesture Detection - Unity Oculus Quest Tutorial, https://www.youtube.com/watch?v=lBzwUKQ3tbw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//struc = class without function
[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognised;
}

//This Class Is Used To Record And Detect Gestures. It Also Can Perform Different Actions Depending On The Gesture Used
public class GestureDetector : MonoBehaviour
{
    [Header("GESTURE DETECTION")]
    [SerializeField]
    private float threshold = 0.1f;

    [SerializeField]
    private OVRSkeleton skeleton;

    [SerializeField]
    private List<Gesture> gestures;

    [SerializeField]
    private bool debugMode = true;

    [SerializeField]
    private  List<OVRBone> fingerBones;

    private Gesture previousGesture;
    private bool isFingerSet;

    [Header("TIME RECORDING")]
    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    //Reset Ring
    public GameObject resetCentre;

    //Reference To the MoveToResetCentre Class
    private MoveToResetCentre moving;

    //Hand Aura
    public GameObject Aura;

    //Shows If The Rewind Mechanic Is Currently Being Used
    private bool duringRewind = false;

    [Header("LEVEL SETUP")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    [Header("TOGGLE AMPLIFIER")]
    //Reference To The ToggleAmplifier Class
    private ToggleAmplifier toggle;

    //Shows If The Movement Mechanic Is Currently Being USed
    public bool isMoving;

    [Header("CONFIRM THUMBS UP")]
    //Shows If The Player Is Currently Forming A Thumbs Up
    public bool isConfirming;

    [Header("BOUNDARY POSITIONS")]
    //Reference To The BoundaryPositions Class
    public GameObject boundary;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Checks If The RecordRotationPosition Class Is Available
        if (GameObject.FindWithTag("PlayerControl") != null)
        {
            //Reference To The RecordRotationPosition Class
            record = GameObject.FindWithTag("PlayerControl").GetComponent<RecordRotationPosition>();

            //Checks If The Reset Centre Object Is Available
            if (resetCentre != null)
            {
                //Reference To The MoveToResetCentre Class
                moving = resetCentre.GetComponent<MoveToResetCentre>();
            }

            //Checks If The Aura Object Is Available
            if (Aura != null)
            {
                Aura.SetActive(false);
            }
        }

        //Checks If The ToggleAmplifier Class Is Available
        if (GameObject.FindWithTag("Toggle") != null)
        {
            toggle = GameObject.FindWithTag("Toggle").GetComponent<ToggleAmplifier>();
        }

        //Checks If The AddExistingCorners Class Is Available
        if (GameObject.FindWithTag("CornerSetter") != null)
        {
            //Reference To The AddExistingCorners Class
            add = GameObject.FindWithTag("CornerSetter").GetComponent<AddExistingCorners>();
        }

        while (skeleton.Bones.Count == 0)
        {
            yield return null;
        }

        fingerBones = new List<OVRBone>(skeleton.Bones);
        isFingerSet = true;
        previousGesture = new Gesture();

        isMoving = false;
        isConfirming = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Saves Current Gesture When In Debug Mode
        if(debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        if (isFingerSet == true)
        {
            //Checks If The Current Gesture Can Be Recognised
            Gesture currentGesture = Recognise();

            bool hasRecognised = !currentGesture.Equals(new Gesture());
            //Check if new gesture
            if (hasRecognised && !currentGesture.Equals(previousGesture))
            {
                if (currentGesture.name != null)
                {
                    //New Gesture
                    previousGesture = currentGesture;
                    currentGesture.onRecognised.Invoke();
                }
            }

            //Checks If The RecordRotationPosition Class And The Aura Object Are Available
            if (GameObject.FindWithTag("PlayerControl") != null && Aura != null)
            {
                //Hides Hand Aura:
                //If The Player Does Not Have Their Palm Out
                //If The Player Is Not Forming A Fist
                //If The Aura Is Visible
                //If The Player Is Not Using The Rewind Mechanic
                if (currentGesture.name != "Palm Out" && currentGesture.name != "Fist" && Aura.activeSelf == true)
                {
                    StartCoroutine(HideAura());
                }

                //Stops The Rewind Mechanic, Hides The Hand Aura, And Sets duringRewind To False:
                //If The Player Is Not Forming A Fist
                //If The Rewind Mechanic Is Still Running
                //If duringRewind Is True
                if (currentGesture.name != "Fist" && record.rewindTrigger == true && duringRewind == true)
                {
                    record.rewindTrigger = false;
                    duringRewind = false;
                    Aura.SetActive(false);
                }
            }

            //Deactivates The Movement Mechanic:
            //If The Player Is Not Forming A Thumbs Up
            //If The Player Is Using The Movement Mechanic
            //If The Player Is Moving Less Than 0.07, and Greater Than -0.07 On The X And Z Axis
            if (currentGesture.name != "ThumbsUp" && 
                isMoving == true && 
               (toggle.playerVelocity.x < 0.07 && toggle.playerVelocity.x > -0.07) &&
               (toggle.playerVelocity.z < 0.07 && toggle.playerVelocity.z > -0.07))
            {
                isMoving = false;
                toggle.disableAmplifiedMovement();
            }

            //Shows That The Player Is Not Forming A Thumbs Up:
            //If The Player Is Not Forming A Thumbs Up
            //If The Project Is Still Showing That The Player IS Forming A Thumbs Up
            if (currentGesture.name != "ThumbsUp" && isConfirming == true)
            {
                isConfirming = false;
            }
        }
    }

    //This Function Hides The Hand Aura After One Second If The Player Is Not Using The Rewind Mechanic
    IEnumerator HideAura()
    {
        yield return new WaitForSeconds(1);

        if(duringRewind == false)
        {
            Aura.SetActive(false);
        }
    }

    //This Function Shows The Hand Aura If The Player Is Not Using The Reset Mechanic
    public void PalmOut()
    {
        if (add.isResetActivated == false)
        {
            Aura.SetActive(true);
        }
        else
        {
            Aura.SetActive(false);
        }
    }

    //This Function Starts The Reset Mechanic, If The Hand Aura Is Visible
    public void ResetFist()
    {
        //Checks If The Hand Aura Is Visible
        if (Aura.activeSelf == true)
        {
            //Starts The Reset Mechanic
            add.isResetActivated = true;

            //Hides All Objects In The Level
            add.ground.SetActive(false);

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

            resetCentre.SetActive(true);
            Aura.SetActive(false);
        }
    }

    //This Function Activates The Rewind Mechanic If The Hand Aura Is Visible
    public void RewindFist()
    {
        if (Aura.activeSelf == true)
        {
            record.rewindTrigger = true;
            duringRewind = true;
        }
    }

    //This Function Activates The Movement Mechanic
    public void StartMoving()
    {
        isMoving = true;
        toggle.enableAmplifiedMovement();
    }

    //This Function Shows That The Player Is Forming A Thumbs Up
    public void ThumbsOut()
    {
        isConfirming = true;
    }

    //This Function Shows Confirms The Set Up Boundary
    public void ConfirmBoundary()
    {
        isConfirming = true;

        if (GameObject.FindWithTag("Level").GetComponent<BoundarySetUp>().cornerNumber == 999)
        {
            //Calculates The Distance Betwen The Player And The Boundary Point
            float distanceBetween = Vector3.Distance(   new Vector3(GameObject.FindWithTag("MainCamera").transform.position.x, 
                                                                    0, 
                                                                    GameObject.FindWithTag("MainCamera").transform.position.z),
                                                        new Vector3(boundary.transform.position.x, 
                                                                    0, 
                                                                    boundary.transform.position.z));

            //Confirms The Set Up Boundary If The Distance Calculated Is Less Than The Radius
            if (distanceBetween <= boundary.GetComponent<BoundaryPositions>().radius)
            {
                boundary.GetComponent<BoundaryPositions>().saveBoundaryPoints();
                boundary.GetComponent<BoundaryPositions>().boundaryMode = false;
            }
        }
    }

    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            //finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture Recognise()
    {
        Gesture currentgesture = new Gesture();
        float currentMin = Mathf.Infinity;

        if (isFingerSet == true)
        {

            foreach (var gesture in gestures)
            {
                float sumDistance = 0;
                bool isDiscarded = false;
                for (int i = 0; i < fingerBones.Count; i++)
                {
                    Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                    float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                    if (distance > threshold)
                    {
                        isDiscarded = true;
                        break;
                    }

                    sumDistance += distance;
                }

                if (!isDiscarded && sumDistance < currentMin)
                {
                    currentMin = sumDistance;
                    currentgesture = gesture;
                }
            }
        }

        return currentgesture;
    }
}
