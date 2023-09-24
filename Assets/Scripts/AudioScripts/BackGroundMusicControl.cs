using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Controls If The Background Music Plays Normally, In Reverse, Or Stops
public class BackGroundMusicControl : MonoBehaviour
{
    [Header("BACKGROUND AUDIO")]
    //References The Background AudioSource
    private AudioSource backgroundAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Initialises The backGround AudioSource
        backgroundAudio = GetComponent<AudioSource>();

        //Plays The Background Music
        if (!backgroundAudio.isPlaying) backgroundAudio.Play();
    }

    //This Function Resets The Background Music
    public void ResetMusic()
    {
        //This Stops And Then Plays The Background Music, Thereby Resetting It To The The Start Of The Track
        if (backgroundAudio.isPlaying) backgroundAudio.Stop();
        if (!backgroundAudio.isPlaying) backgroundAudio.Play();
    }

    //This Function Rewinds The Background Music
    public void RewindMusic()
    {
        //This Reverses The Track
        backgroundAudio.pitch = -1;
    }

    //This Function Plays The Background Music Normally
    public void ForwardMusic()
    {
        //This Plays The Track Forward
        backgroundAudio.pitch = 1;
    }

    //This Function Pauses The Background Music
    public void StopMusic()
    {
        //This Pauses The Track
        backgroundAudio.pitch = 0;
    }
}
