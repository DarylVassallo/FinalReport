using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Checks If The Player Can Go To The Next Level
public class ReadyNextLevel : MonoBehaviour
{
    [Header("NEXT LEVEL")]
    //The Name Of The Next Level
    public string nextLevel;

    [Header("PLAYER")]
    //Player's Camera
    private Transform player;

    [Header("TOGGLEAMPLIFIER")]
    //Reference To The ToggleAmplifier Class
    private ToggleAmplifier toggle;

    //Stores The Distance Between The Player and The Exit Ring
    private float distanceBetween;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The ToggleAmplifier
        toggle = GameObject.FindGameObjectWithTag("Toggle").GetComponent<ToggleAmplifier>();

        //Player's Camera
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //Sets The Position And Scale Of The Movement Ring
        transform.position = new Vector3(0, player.position.y - 0.5f, 0);
        transform.localScale = new Vector3(toggle.radius * 1.5f, transform.localScale.y, toggle.radius * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Keeps The Exit Object In Line With The Player's Waist
        transform.position = new Vector3(0, player.position.y - 0.5f, 0);

        //Calculates The Distance Betwen The Player And The Exit Ring
        distanceBetween = Vector3.Distance( new Vector3(player.position.x, 0, player.position.z),
                                            new Vector3(transform.position.x, 0, transform.position.z));

        //Shows That The Player Can Go To the Next Level:
        //If The Player Is Within The Range To The Exit Object On The X And Z Axis
        if (distanceBetween <= (toggle.radius * 0.75f))
        {
            toggle.canPlayerLeave = true;
        }else{
            toggle.canPlayerLeave = false;
        }
    }
}
