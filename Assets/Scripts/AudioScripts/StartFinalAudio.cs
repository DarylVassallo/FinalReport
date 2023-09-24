using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The Final Level, And Checks When To Output Them
public class StartFinalAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //The Audio Clip Used For The Final Level
    public AudioClip Final1;
    public AudioClip Final2;

    //Reference To The AudioSource Component
    private AudioSource finalAudio;

    //Shows The Current Audio Clip Used
    private int audioSteps = 1;

    //Narrator Subtitles
    public TMP_Text narratorText;

    //Shows If The Audio Line Has Ended
    private bool startAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The AudioSource Component
        finalAudio = GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'FinalLevel', Before Playing The Audio Clip
        finalAudio.clip = Final1;
        narratorText.text = "Congratulations, you have managed to fix time and space! This concludes your volunteer work, and as a reward, you will receive " +
                            "a generous 3 Euro Discount on your next time travel product.";
        if (!finalAudio.isPlaying) finalAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks If The Audio Should Start
        if (startAudio == true)
        {
            //Uses Specific Audio Clips And Text, Depending On The Value Of audioSteps
            switch (audioSteps)
            {
                case 2:
                    finalAudio.clip = Final2;
                    narratorText.text = "Thank you for participating, and I hope you enjoyed yourself.";
                    break;               
            }

            //Checks If The Last Audio Clip Was Played
            if (audioSteps != 3)
            {
                //Plays The Audio Clip
                if (!finalAudio.isPlaying) finalAudio.Play();

                //Shows That The Audio Clip Should Not Be Played Again
                startAudio = false;
            }

        }

        //Checks If audioSteps Is Equal To 1 And If The Final1 Audio Has Ended
        if ((audioSteps == 1 && finalAudio.isPlaying == false))
        {
            //Increments audioSteps By 1
            audioSteps++;

            //Shows That The Audio Should Start
            startAudio = true;
        }
    }
}
