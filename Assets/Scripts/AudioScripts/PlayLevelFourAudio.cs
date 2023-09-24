using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Class Outputs All The Narrator's Voice Lines In The Four Level, And Checks When To Output Them
public class PlayLevelFourAudio : MonoBehaviour
{
    [Header("NARRATOR AUDIO AND TEXT")]
    //The Audio Clip Used For Level Four
    public AudioClip LevelFour;

    //Reference To The AudioSource Component
    private AudioSource levelFourAudio;

    //Reference To The Music AudioSource Component
    private AudioSource levelMusic;

    //Narrator Subtitles
    public TMP_Text narratorText;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The AudioSource Component
        levelFourAudio = GetComponent<AudioSource>();

        //Reference To The Background Music's AudioSource Component
        levelMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();

        //Sets The Current Audio Clip And Text To 'LevelFour', Before Playing The Audio Clip
        levelFourAudio.clip = LevelFour;
        narratorText.text = "Time’s still not fixed, so get to fixing!";
        if (!levelFourAudio.isPlaying) levelFourAudio.Play();

        //Sets The Game Music To Volume 0
        levelMusic.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets The Game Music to 0.5 If The Narrator Is Not Speaking
        if (!levelFourAudio.isPlaying)
        {
            levelMusic.volume = 0.5f;

        //Otherwise The Game Music Is Set To 0
        }else{
            levelMusic.volume = 0;
        }
    }
}
