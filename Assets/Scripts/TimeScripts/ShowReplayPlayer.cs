using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class ShowReplayPlayer : MonoBehaviour
{
    //Reference To The RecordRotationPosition Class
    private GameObject playerControl;
    private RecordRotationPosition record;

    //Replay Player Body Parts
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject vrHead;

    //Visual Models Of The Right And Left Hand
    public HandVisual rightHandVisual;
    public HandVisual leftHandVisual;

    // Start is called before the first frame update
    void Start()
    {
        //Initialises The Reference To The RecordRotationPosition Class
        playerControl = GameObject.FindWithTag("PlayerControl");
        record = playerControl.GetComponent<RecordRotationPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stores Values For The Object Grabbed By The Left And Right Hand Into Their Respective Variables
        PastGrabObject grabObjectLeft = leftHand.GetComponent<PastGrabObject>();
        PastGrabObject grabObjectRight = rightHand.GetComponent<PastGrabObject>();

        //Checks If Either Rewind Button Has Been Pressed
        if (record.rewindTrigger == true)
        {
            //Checks If The Length Of The List (Such As headPosition From RecordPositionRotation), Is Greater Or Equal To 5
            if(record.headPosition.Count >= 5)
            {
                //The Rewind Player's Head Is Set To The Last Recorded Position And Rotation Using The Respective Lists
                vrHead.transform.position = record.headPosition[record.headPosition.Count - 1];
                vrHead.transform.rotation = record.headRotation[record.headRotation.Count - 1];

                //The Rotation Of The Replay Player's Head Is Tweaked To Put It In The Correct Rotation
                vrHead.transform.Rotate(new Vector3(-90, 0, 0));

                for (int i = 0; i < rightHandVisual._jointTransforms.Count; i++)
                {
                    //The Rewind Player's Right Hand Is Set To The Last Recorded Position And Rotation Using The Respective Lists
                    rightHandVisual._jointTransforms[i].position = record.rightFingerBonePosition[record.rightFingerBonePosition.Count - 1][i];
                    rightHandVisual._jointTransforms[i].rotation = record.rightFingerBoneRotation[record.rightFingerBoneRotation.Count - 1][i];

                    //The Rewind Player's Left Hand Is Set To The Last Recorded Position And Rotation Using The Respective Lists
                    leftHandVisual._jointTransforms[i].position = record.leftFingerBonePosition[record.leftFingerBonePosition.Count - 1][i];
                    leftHandVisual._jointTransforms[i].rotation = record.leftFingerBoneRotation[record.leftFingerBoneRotation.Count - 1][i];
                }
                leftHand.transform.position = leftHandVisual._jointTransforms[0].position;
                rightHand.transform.position = rightHandVisual._jointTransforms[0].position;
            }

        }else{
            //The Replay Player Is Destroyed
            Destroy(this.gameObject);
        }
    }
}
