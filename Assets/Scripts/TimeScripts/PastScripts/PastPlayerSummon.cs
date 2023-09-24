using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Spawns The Full Past Player Teleport Effect Whenever It Is Needed 
public class PastPlayerSummon : MonoBehaviour
{
    //Reference To The ParticleSystem Component
    private ParticleSystem effect;

    //Reference To The AudioSource
    private AudioSource teleportAudio;

    //The Past Player's Body
    public GameObject pastBody;

    //Shows The Previous Active Stat Of The Past Player's Body
    private bool prevActive;

    // Start is called before the first frame update
    void Start()
    {
        //Initialises The Reference To The ParticleSystem Component
        effect = GetComponent<ParticleSystem>();

        //Initialises The Reference To The AudioSource Component
        teleportAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks If The Past Player's Body Is Active
        if(pastBody.activeSelf == true)
        {
            //The Current Position Is Set To That Of The Past Player's Body
            this.transform.position = pastBody.transform.position;
        }
        
        //Checks If The Past Player's Body Is Not Active, And If The Previous Active State Was Active
        if(pastBody.activeSelf == false && prevActive == true)
        {
            //Plays The Teleport Particles And Audio
            if (!effect.isPlaying) effect.Play();
            if (!teleportAudio.isPlaying) teleportAudio.Play();
        }

        //The Current Active State Of The Past Player's Body Is Stored In prevActive
        prevActive = pastBody.activeSelf;
    }
}
