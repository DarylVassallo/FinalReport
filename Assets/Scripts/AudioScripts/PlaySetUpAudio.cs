using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The SetUp Level, And Checks When To Output Them
public class PlaySetUpAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //All Audio Clips Used For The SetUp Level
    [SerializeField]
    private AudioClip SetUp1;
    [SerializeField]
    private AudioClip SetUp2;
    [SerializeField]
    private AudioClip SetUp3;

    //Reference To The AudioSource
    private AudioSource setUpAudio;

    //Shows The Current Audio Clip Used
    private int audioSteps = 1;

    //Shows If The Audio Line Has Ended
    private bool startAudio = false;

    //Narrator Subtitles
    [SerializeField]
    private TMP_Text narratorText;

    [Header("RESET PLAYER POSITION")]
    //This Is The Position The Player Will Be Set To When Reset
    [SerializeField]
    private Transform resetTransform;

    //Player's Camera
    private GameObject player;

    //Player's Camera Parent
    private GameObject playerBody;

    //Shows If The Player's Headset Is Being Tracked
    private bool headIsTracking;

    [Header("BOUNDARY SETUP")]
    private BoundarySetUp boundary;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The Player's Camera, And It's Parent
        player = GameObject.FindGameObjectWithTag("MainCamera");
        playerBody = GameObject.FindGameObjectWithTag("OVRCameraRig");
        boundary = GameObject.FindGameObjectWithTag("Level").GetComponent<BoundarySetUp>();

        //Initialises The Reference To The AudioSource Component
        setUpAudio = GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'SetUp1', Before Playing The Audio Clip
        setUpAudio.clip = SetUp1;
        narratorText.text = "Good, now you can point to the floor and select where a corner should be. " +
                            "You may need to look under your VR headset to see where the corner is going to be in the physical area.";
        if (!setUpAudio.isPlaying) setUpAudio.Play();

        headIsTracking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Resets The Player's Position, And Rotation, Before Showing That The Player Is Being Tracked:
        //If headIsTracking Shows That The Player Is Not Being Tracked
        //If The Player's X And Z Rotation Angles Are Not Equal To 0
        if (headIsTracking == false && (player.transform.rotation.eulerAngles.x != 0 || player.transform.rotation.eulerAngles.z != 0))
        {
            boundary.playerRotation = resetTransform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y;
            playerBody.transform.Rotate(0, boundary.playerRotation, 0);

            headIsTracking = true;
        }

        //Checks If The Audio Should Start
        if (startAudio == true)
        {
            //Uses Specific Audio Clips And Text, Depending On The Value Of audioSteps
            switch (audioSteps)
            {
                case 2:
                    setUpAudio.clip = SetUp2;
                    narratorText.text = "Now select where the diagonally opposite corner should be.";
                    break;
                case 3:
                    setUpAudio.clip = SetUp3;
                    narratorText.text = "The area has now been confirmed, if you wish to repeat this process, select the walls of the area. " +
                                        "Otherwise, move to the centre of the green rings, face the front of your physical space, " +
                                        "and form a thumbs up.";
                    break;
            }

            //Checks If The Last Audio Clip Was Played
            if (audioSteps != 4)
            {
                //Plays The Audio Clip
                if (!setUpAudio.isPlaying) setUpAudio.Play();

                //Shows That The Audio Clip Should Not Be Played Again
                startAudio = false;
            }

        }

        //Checks If audioSteps Is Equal To 1, If The SetUp1 Audio Has Ended, And The First Corner Has Been Placed
        if ((audioSteps == 1 && setUpAudio.isPlaying == false && boundary.cornerNumber == 4) ||

            //Or Checks If audioSteps Is Equal To 2, If The SetUp2 Audio Has Ended, And The Second Corner Has Been Placed
            (audioSteps == 2 && setUpAudio.isPlaying == false && boundary.cornerNumber == 999))
        {
            //Increments audioSteps By 1
            audioSteps++;

            //Shows That The Audio Should Start
            startAudio = true;
        }
    }
}