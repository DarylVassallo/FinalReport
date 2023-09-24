using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The Third Level, And Checks When To Output Them
public class PlayLevelThreeAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //All Audio Clips Used For Level Three
    public AudioClip LevelThree1Part1;
    public AudioClip LevelThree1Part2;
    public AudioClip LevelThree2;
    public AudioClip LevelThree3Part1;
    public AudioClip LevelThree3Part2;
    public AudioClip LevelThree4Part1;
    public AudioClip LevelThree4Part2;

    //Reference To The AudioSource
    private AudioSource levelThreeAudio;

    //Reference To The Music AudioSource
    private AudioSource levelMusic;

    //Shows The Current Audio Clip Used
    private int audioSteps = 1;

    //Shows If The Audio Line Has Ended
    private bool startAudio = false;

    //Narrator Subtitles
    public TMP_Text narratorText;

    [Header("GESTURE DETECTORS")]
    //Gesture Detectors For The Right And Left Hand
    public GestureDetector leftGesture;
    public GestureDetector rightGesture;

    [Header("ADD EXISTING CORNERS")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    [Header("RECORD ROTATION POSITION")]
    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    public AudioSource retryAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The AddExistingCorners Class
        add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Reference To The RecordRotationPosition Class
        record = GameObject.FindGameObjectWithTag("PlayerControl").GetComponent<RecordRotationPosition>();

        //Reference To The AudioSource Component
        levelThreeAudio = GetComponent<AudioSource>();

        //Reference To The Background Music's AudioSource Component
        levelMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'LevelThree1Part1', Before Playing The Audio Clip
        levelThreeAudio.clip = LevelThree1Part1;
        narratorText.text = "Congratulations, you have successfully merged all universes into one. Unfortunately, you have also completely broken time, which " +
                            "is pretty impressive.";
        if (!levelThreeAudio.isPlaying) levelThreeAudio.Play();

        //Sets The Game Music To Volume 0
        levelMusic.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets The Game Music To 0.5 If The Narrator Is Not Speaking
        if (!levelThreeAudio.isPlaying && !retryAudio.isPlaying)
        {
            levelMusic.volume = 0.5f;

        //Otherwise The Game Music Is Set To 0
        }else{
            levelMusic.volume = 0;
        }

        //Checks If The Audio Should Start
        if (startAudio == true)
        {
            //Uses Specific Audio Clips And Text, Depending On The Value Of audioSteps
            switch (audioSteps)
            {
                case 2:
                    levelThreeAudio.clip = LevelThree1Part2;
                    narratorText.text = "Here, I have allowed you to reset, and rewind time. Try Resetting time, by opening, and then closing your right Hand.";
                    break;
                case 3:
                    levelThreeAudio.clip = LevelThree2;
                    narratorText.text = "If you are now seeing nothing, then turn around, because there is a ring in the center of the room. Go to the ring, " +
                                        "and it will take you back to the moment you entered the room.";
                    break;
                case 4:
                    levelThreeAudio.clip = LevelThree3Part1;
                    narratorText.text = "You have successfully reset time, but this does not mean that your actions have been undone, You can now see yourself " +
                                        "from the past, doing everything you just did.";
                    break;
                case 5:
                    levelThreeAudio.clip = LevelThree3Part2;
                    narratorText.text = "Try opening and closing your left hand to rewind time. Make sure to keep your hand closed.";
                    break;
                case 6:
                    levelThreeAudio.clip = LevelThree4Part1;
                    narratorText.text = "Now all objects and past versions are moving in reverse. This will continue until you open your hand again.";
                    break;
                case 7:
                    levelThreeAudio.clip = LevelThree4Part2;
                    narratorText.text = "Using your new found time powers, try and fix this room.";
                    break;
            }

            //Checks If The Last Audio Clip Was Played
            if (audioSteps != 8)
            {
                //Plays The Audio Clip
                if (!levelThreeAudio.isPlaying) levelThreeAudio.Play();

                //Shows That The Audio Clip Should Not Be Played Again
                startAudio = false;
            }

        }

        //Checks If audioSteps Is Equal To 1 And If The LevelThree1Part1 Audio Has Ended
        if ((audioSteps == 1 && levelThreeAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 2, If The LevelThree1Part2 Audio Has Ended, And If The Reset Mechanic Is Being Used
            (audioSteps == 2 && levelThreeAudio.isPlaying == false && add.isResetActivated == true) ||

            //Or Checks If audioSteps Is Equal To 3, If The LevelThree2 Audio Has Ended, And If The Reset Mechanic Is Not Being Used
            (audioSteps == 3 && levelThreeAudio.isPlaying == false && add.isResetActivated == false) ||

            //Or Checks If audioSteps Is Equal To 4, And If The LevelThree3Part1 Audio Has Ended
            (audioSteps == 4 && levelThreeAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 5, If The LevelThree3Part2 Audio Has Ended, And If The Rewind Mechanic Is Being Used
            (audioSteps == 5 && levelThreeAudio.isPlaying == false && record.rewindTrigger == true) ||

            //Or Checks If audioSteps Is Equal To 6, If The LevelThree4Part1 Audio Has Ended
            (audioSteps == 6 && levelThreeAudio.isPlaying == false))
        {
            //Increments audioSteps By 1
            audioSteps++;

            //Shows That The Audio Should Start
            startAudio = true;
        }
    }
}