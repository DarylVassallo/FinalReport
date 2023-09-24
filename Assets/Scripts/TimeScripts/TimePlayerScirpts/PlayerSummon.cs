using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Spawns The Full Player Teleport Effect Whenever It Is Needed 
public class PlayerSummon : MonoBehaviour
{
    //Reference To The ParticleSystem Component
    private ParticleSystem effect;

    //Reference To The AudioSource
    private AudioSource teleportAudio;

    //Reference To The RecordRotationPosition Class
    private RecordRotationPosition record;

    // Start is called before the first frame update
    void Start()
    {
        //Initialises The Reference To The ParticleSystem Component
        effect = GetComponent<ParticleSystem>();

        //Initialises The Reference To The AudioSource Component
        teleportAudio = GetComponent<AudioSource>();

        //Reference To The RecordRotationPosition Class
        record = GameObject.FindWithTag("PlayerControl").GetComponent<RecordRotationPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks If The Player Has Reset Their Position
        if (record.resetPosition == true)
        {
            //Plays The Teleport Particles And Audio
            if (!effect.isPlaying) effect.Play();
            if (!teleportAudio.isPlaying) teleportAudio.Play();

            record.resetTrigger = false;
        }
    }
}
