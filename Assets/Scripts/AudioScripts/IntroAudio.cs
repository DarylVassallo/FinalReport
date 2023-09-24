using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The Introduction Level, And Checks When To Output Them
public class IntroAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //All Audio Clips Used For The Introduction Level
    public AudioClip Intro1Part1;
    public AudioClip Intro1Part2;
    public AudioClip Intro1Part3;
    public AudioClip Intro2;
    public AudioClip Intro3;
    public AudioClip Intro4Part1;
    public AudioClip Intro4Part2;

    //Reference To The AudioSource
    private AudioSource introductionAudio;

    //Shows The Current Audio Clip Used
    private int audioSteps = 1;

    //Shows If The Audio Line Has Ended
    private bool startAudio = false;

    //Narrator Subtitles
    public TMP_Text narratorText;

    //Shows If The Player Is Hovering Over The Sign
    private bool isHovering;

    //Shows If The Player Is Selecting The Sign
    private bool isSelecting;

    [Header("GESTURE DETECTORS")]
    //Gesture Detectors For The Right And Left Hand
    public GestureDetector leftGesture;
    public GestureDetector rightGesture;

    [Header("RESET PLAYER POSITION")]
    //Player's Camera
    private Transform player;

    //Player's Camera Parent
    private Transform playerBody;

    //Shows If The Player's Headset Is Being Tracked
    private bool headIsTracking;

    //This Is The Position The Player Will Be Set To When Reset
    public Transform resetTransform;

    //Reference To The Renderer Component
    private Renderer signRenderer;

    //Shows The Transparency Value Of The Sign
    private float signTransparency;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The Player's Camera, And It's Parent
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        playerBody = GameObject.FindGameObjectWithTag("OVRCameraRig").transform;

        //Initialises The Reference To The AudioSource Component
        introductionAudio = GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'Intro1Part1', Before Playing The Audio Clip
        introductionAudio.clip = Intro1Part1;
        narratorText.text = "Hello, if you are hearing this, then it means that you have been accepted to participate in mandatory " +
                            "volunteer work to fix a quantum machine. I am very excited, as you can hear from the tone of my voice.";
        if (!introductionAudio.isPlaying) introductionAudio.Play();

        //Sets The Sign To Be Transparent
        signRenderer = GetComponent<Renderer>();
        signRenderer.material.color = new Color(signRenderer.material.color.r, 
                                                signRenderer.material.color.g, 
                                                signRenderer.material.color.b, 0);

        headIsTracking = false;

        isHovering = false;
        isSelecting = false;

        signTransparency = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //Resets The Player's Position, And Rotation, Before Showing That The Player Is Being Tracked:
        //If headIsTracking Shows That The Player Is Not Being Tracked
        //If The Player's X And Z Rotation Angles Are Not Equal To 0
        if (headIsTracking == false && (player.rotation.eulerAngles.x != 0 || player.rotation.eulerAngles.z != 0))
        {
            var rotationAngleY = resetTransform.rotation.eulerAngles.y - player.rotation.eulerAngles.y;
            playerBody.Rotate(0, rotationAngleY, 0);

            headIsTracking = true;
        }

        //When The Sign Is Going To Be Used, It Gradually Becomes Opaque
        if (audioSteps >= 3 && signTransparency < 1)
        {
            StartCoroutine(FadeInSign(1f));
        }

        //Checks If The Audio Should Start
        if (startAudio == true)
        {
            //Uses Specific Audio Clips And Text, Depending On The Value Of audioSteps
            switch (audioSteps)
            {
                case 2:
                    introductionAudio.clip = Intro1Part2;
                    narratorText.text = "Before starting, we need to ensure your VR hands are working. ";
                    break;
                case 3:
                    introductionAudio.clip = Intro1Part3;
                    narratorText.text = "There should now be a sign in front of you, try pointing at it.";
                    break;
                case 4:
                    introductionAudio.clip = Intro2;
                    narratorText.text = "Good, now try and select the sign, by pinching.";
                    break;
                case 5:
                    introductionAudio.clip = Intro3;
                    narratorText.text = "This is great, now we need to be sure that you are capable of using your VR fingers. " +
                                        "Try and form a thumbs up";
                    break;
                case 6:
                    introductionAudio.clip = Intro4Part1;
                    narratorText.text = "Congratulations, you have a skill only the vast majority of people possess. You can use this " +
                                        "if you need to show confirmation.";
                    break;
                case 7:
                    introductionAudio.clip = Intro4Part2;
                    narratorText.text = "We now need to mark out a quadrilateral area to represent your available space. When you are ready, " +
                                        "face the front of your physical area, and form a thumbs up. ";
                    break;
            }

            //Checks If The Last Audio Clip Was Played
            if (audioSteps != 8)
            {
                //Plays The Audio Clip
                if (!introductionAudio.isPlaying) introductionAudio.Play();

                //Shows That The Audio Clip Should Not Be Played Again
                startAudio = false;
            }
            else
            {
                //Sends the Player to The Level 'SetUpLevel'
                SceneManager.LoadScene("SetUpLevel");
            }

        }

        //Checks If audioSteps Is Equal To 1, And If The Intro1Part1 Audio Has Ended
        if ((audioSteps == 1 && introductionAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 2, And If The Intro1Part2 Audio Has Ended
            (audioSteps == 2 && introductionAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 3, If The Intro1Part3 Audio Has Ended, And If The Player Is Hovering Over The Sign
            (audioSteps == 3 && introductionAudio.isPlaying == false && isHovering == true) ||

            //Or Checks If audioSteps Is Equal To 4, If The Intro2 Audio Has Ended, And If The Player Is Selecting The Sign
            (audioSteps == 4 && introductionAudio.isPlaying == false && isSelecting == true) ||

            //Or Checks If audioSteps Is Equal To 5, If The Intro3 Audio Has Ended, And If The Player Has Formed A Thumbs Up
            (audioSteps == 5 && introductionAudio.isPlaying == false && (leftGesture.isConfirming == true || rightGesture.isConfirming == true)) ||

            //Or Checks If audioSteps Is Equal To 6, And If The Intro4Part1 Audio Has Ended
            (audioSteps == 6 && introductionAudio.isPlaying == false) ||

            //Or Checks If audioSteps Is Equal To 7, If The Intro4Part2 Audio Has Ended, And If The Player Has Formed A Thumbs Up
            (audioSteps == 7 && introductionAudio.isPlaying == false && (leftGesture.isConfirming == true || rightGesture.isConfirming == true)))
        {
            //Increments audioSteps By 1
            audioSteps++;

            //Shows That The Audio Should Start
            startAudio = true;
        }
    }

    //This Function Fades The Sign Into Visible View
    IEnumerator FadeInSign(float fadeSpeed)
    {
        float alphaValue = 0f;

        //Repeats The Section Until The Sign Is Visible
        while (signRenderer.material.color.a < 1f)
        {
            //Slowly Fades The Sign Into View
            alphaValue += Time.deltaTime / fadeSpeed;
            signRenderer.material.color = new Color(signRenderer.material.color.r,
                                                    signRenderer.material.color.g,
                                                    signRenderer.material.color.b, alphaValue);

            signTransparency = alphaValue;

            yield return null;
        }
    }

    //This Function Shows If The Player Is Hovering Over The Sign
    public void HoverOverSign()
    {
        isHovering = true;
    }

    //This Function Shows If The Player Is Not Hovering Over The Sign
    public void UnHoverOverSign()
    {
        isHovering = false;
    }

    //This Function Shows If The Player Is Selecting The Sign
    public void SelectSign()
    {
        isSelecting = true;
    }

    //This Function Shows If The Player Is Not Selecting The Sign
    public void UnSelectSign()
    {
        isSelecting = false;
    }
}