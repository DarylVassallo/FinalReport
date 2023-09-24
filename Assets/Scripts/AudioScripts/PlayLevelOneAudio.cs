using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The First Level, And Checks When To Output Them
public class PlayLevelOneAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //All Audio Clips Used For Level One
    public AudioClip LevelOne1Part1;
    public AudioClip LevelOne1Part2;
    public AudioClip LevelOne1Part3;
    public AudioClip LevelOne2Part1;
    public AudioClip LevelOne2Part2;
    public AudioClip LevelOne3;
    public AudioClip LevelOne4;
    public AudioClip LevelOne5;
    public AudioClip LevelOne6;

    //Reference To The AudioSource
    private AudioSource levelOneAudio;

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

    [Header("PLAYER OBJECTIVES")]
    //Shows If The Player Has Selected The Floor
    private bool hasSelected;

    //Shows If The Player Has Reached The Box
    private bool hasReachedBox;

    //Shows If The Player Has Activated The Button
    private bool hasActivatedButton;

    //Box GameObject
    public Transform box;

    //Range The Player Must Be Within To Have Reached The Box
    public float xzrange;

    [Header("TOGGLEAMPLIFIER")]
    //Reference To the ToggleAmplifier Class
    private ToggleAmplifier toggle;

    //Reference To The Toggle's Renderer Component
    private Renderer toggleRenderer;

    [Header("OVRCAMERARIG")]
    //Reference To The OVRCameraRig Class
    private OVRCameraRig ovr;

    //Shows The Visibility Value Of The Toggle Object
    private float toggleTransparency;

    [Header("BUTTON")]
    //Button GameObject
    private GameObject button;

    [Header("PLAYER")]
    //Player's Main Camera
    private Transform player;

    public AudioSource retryAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The ToggleAmplifier Class
        toggle = GameObject.FindGameObjectWithTag("Toggle").GetComponent<ToggleAmplifier>();

        //Reference To The Toggle's Renderer Component
        toggleRenderer = GameObject.FindGameObjectWithTag("Toggle").GetComponent<Renderer>();

        //Reference To The OVRCameraRig Class
        ovr = GameObject.FindGameObjectWithTag("OVRCameraRig").GetComponent<OVRCameraRig>();

        //Sets The Toggle To Be Transparent
        toggleRenderer.material.color = new Color(  toggleRenderer.material.color.r,
                                                    toggleRenderer.material.color.g,
                                                    toggleRenderer.material.color.b, 0);

        //Shows The Visibility Value Of The Toggle Object
        toggleTransparency = 0;

        //Reference To The Button GameObject
        button = GameObject.FindGameObjectWithTag("Button");

        //Reference To The Player's Camera
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        hasSelected = false;
        hasReachedBox = false;
        hasActivatedButton = false;

        //Reference To The AudioSource Component
        levelOneAudio = GetComponent<AudioSource>();

        //Reference To The Background Music's AudioSource Component
        levelMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'LevelOne1Part1', Before Playing The Audio Clip
        levelOneAudio.clip = LevelOne1Part1;
        narratorText.text = "This is very strange. There should be a path that leads to a box, but only parts of it are here. " +
                            "I will need to do a full evaluation wait here.";
        if (!levelOneAudio.isPlaying) levelOneAudio.Play();

        //Sets The Game Music To Volume 0
        levelMusic.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Shows The Button Has Been Activated:
        //If The Button Has Been Activated
        if (button.GetComponent<ButtonActivation>().activated == true)
        {
            hasActivatedButton = true;
        }

        //Shows The Player Has Reached The Box:
        //If The Player Is Within A Range To The Box On The X And Z Axis
        if (player.position.x >= (box.position.x - xzrange) && player.position.x <= (box.position.x + xzrange) &&
            player.position.z >= (box.position.z - xzrange) && player.position.z <= (box.position.z + xzrange))
        {
            hasReachedBox = true;
        }

        //Sets The Game Music to 0.5 If The Narrator Is Not Speaking
        if (!levelOneAudio.isPlaying && !retryAudio.isPlaying)
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
                    levelOneAudio.clip = LevelOne1Part2;
                    narratorText.text = "Evaluation complete. The machine has scattered each room among three separate universes.";
                    break;
                case 3:
                    levelOneAudio.clip = LevelOne1Part3;
                    narratorText.text = "Fret not, bamboozled volunteer, I am allowing you to cross into different universes. " +
                                        "Try selecting a point on the floor away from you.";
                    break;
                case 4:
                    levelOneAudio.clip = LevelOne2Part1;
                    narratorText.text = "Great, you can use your other hand to cross into a different universe. It may seem that " +
                                        "this room is far larger than your actual physical space. Worry not, for you can use what is " +
                                        "nicknamed as. The Ring.";
                    break;
                case 5:
                    levelOneAudio.clip = LevelOne2Part2;
                    narratorText.text = "Here, when you want to use it, go inside the ring, and give a thumbs up.";
                    break;
                case 6:
                    levelOneAudio.clip = LevelOne3;
                    narratorText.text = "Now you can reach any part of the room by physically walking. When you want to stop using. The Ring. " +
                                        "Just stop holding the thumbs up and stop moving around. Use this power to reach the box.";
                    break;
                case 7:
                    levelOneAudio.clip = LevelOne4;
                    narratorText.text = "Now grab the box and place it on the button.";
                    break;
                case 8:
                    levelOneAudio.clip = LevelOne5;
                    narratorText.text = "Well Done! Now when you are ready to reach the next room, get into the centre of the green rings, which " +
                                        "are also at the centre of the room.";
                    break;
                case 9:
                    levelOneAudio.clip = LevelOne6;
                    narratorText.text = "Face the front of the room, and give a thumbs up.";
                    break;
            }

            //Checks If The Last Audio Clip Was Played
            if (audioSteps != 10 && retryAudio.isPlaying == false)
            {
                //Plays The Audio Clip
                if (!levelOneAudio.isPlaying) levelOneAudio.Play();

                //Shows That The Audio Clip Should Not Be Played Again
                startAudio = false;
            }

        }

        if (audioSteps == 4 && !levelOneAudio.isPlaying)
        {
            StartCoroutine(FadeInToggle(5f));
        }

        //Checks If audioSteps Is Equal To 1 And If The LevelOne1Part1 Audio Has Ended
        if ((audioSteps == 1 && levelOneAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 2 And If The LevelOne1Part2 Audio Has Ended
            (audioSteps == 2 && levelOneAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 3, If The LevelOne1Part3 Audio Has Ended, And If The Player Is Selected The Floor
            (audioSteps == 3 && levelOneAudio.isPlaying == false && hasSelected == true) ||

            //Or Checks If audioSteps Is Equal To 4, And If The LevelOne2Part1 Audio Has Ended
            (audioSteps == 4 && levelOneAudio.isPlaying == false && toggleTransparency >= 0.5) ||

            //Or Checks If audioSteps Is Equal To 5, If The LevelOne2Part2 Audio Has Ended, And If The Movement Mechanic Has Been Activated
            (audioSteps == 5 && levelOneAudio.isPlaying == false && ovr.canAmplify == true) ||

            //Or Checks If audioSteps Is Equal To 6, If The LevelOne3 Audio Has Ended, And If The Player Has Reached The Box
            (audioSteps == 6 && levelOneAudio.isPlaying == false && hasReachedBox == true) ||

            //Or Checks If audioSteps Is Equal To 7, If The LevelOne4 Audio Has Ended, And If The Button Has Been Activated
            (audioSteps == 7 && levelOneAudio.isPlaying == false && hasActivatedButton == true) ||

            //Or Checks If audioSteps Is Equal To 8, If The LevelOne5 Audio Has Ended, And If The Player Is Within The Exit GameObject
            (audioSteps == 8 && levelOneAudio.isPlaying == false && toggle.canPlayerLeave == true))
        {
            //Increments audioSteps By 1
            audioSteps++;

            //Shows That The Audio Should Start
            startAudio = true;
        }
    }

    //This Function Fades The Toggle Object Into Visible View
    IEnumerator FadeInToggle(float fadeSpeed)
    {
        float alphaValue = 0f;

        //Repeats The Section Until The Toggle Object Is Visible
        while (toggleRenderer.material.color.a < 0.5f)
        {
            //Slowly Fades The Toggle Object Into View
            alphaValue += Time.deltaTime / fadeSpeed;
            toggleRenderer.material.color = new Color(  toggleRenderer.material.color.r,
                                                        toggleRenderer.material.color.g,
                                                        toggleRenderer.material.color.b, alphaValue);

            toggleTransparency = alphaValue;

            yield return null;
        }
    }

    //This Function Shows If The Player Has Selected The Floor
    public void selectedFloor()
    {
        hasSelected = true;
    }
}