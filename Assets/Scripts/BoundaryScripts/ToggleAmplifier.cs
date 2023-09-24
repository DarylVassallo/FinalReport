using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

//This Class Enables And Disables The Movement Mechanic
public class ToggleAmplifier : MonoBehaviour
{
    [Header("MOVEMENT MECHANIC")]
    //Minimum Distance Player Needs To Be With The Movement Ring To Enable The Movement Mechanic
    public float radius;

    //Stores The Distance Between The Player and The Movement Ring
    private float distanceBetween;

    //Shows If The Movement Mechanic Is Currently Enabled
    private bool movementEnabled;

    [Header("PLAYER")]
    //Player Camera
    private Transform player;

    [Header("OVRCAMERARIG")]
    //Reference To The OVRCameraRig Class
    private OVRCameraRig ovr;

    [Header("NEXT LEVEL")]
    //Reference To The ReadyNextLevel Class
    private ReadyNextLevel ready;

    //Shows If The Player Can Go To The Next Level
    public bool canPlayerLeave;

    [Header("VELOCITY")]
    //Stores The Last Recorded Position Of The Player
    private Vector3 previousPos;

    //Stores The Velocity Of The Player
    public Vector3 playerVelocity;

    private Vector3 posDiff;
    // Start is called before the first frame update
    void Start()
    {
        //Player Camera
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //Reference To The OVRCameraRig Class
        ovr = GameObject.FindGameObjectWithTag("OVRCameraRig").GetComponent<OVRCameraRig>();

        //Sets The Position And Scale Of The Movement Ring
        transform.parent.transform.position = new Vector3(0, player.position.y - 0.5f, 0);
        transform.parent.transform.localScale = new Vector3(radius * 2, transform.localScale.y, radius * 2);

        //Stores Current Position Of The Player
        previousPos = player.transform.position;

        //Shows That The Movement Mechanic Is Not Being Used
        movementEnabled = false;        
    }

    // Update is called once per frame
    void Update()
    {
        //Keeps The Movement Ring In Line To The Player's Waist
        transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, player.position.y - 0.5f, transform.parent.transform.position.z);

        //Keeps The Movement Ring Near The Player:
        //If The Movement Mechanic Is Enabled
        if (movementEnabled == true)
        {
            transform.parent.transform.position = new Vector3(  player.position.x + posDiff.x, 
                                                                player.position.y - 0.5f, 
                                                                player.position.z + posDiff.z);
        }

        //Uses The Player's Current Position And Last Recorded Position To Calculate The Player's Velocity
        playerVelocity = (player.transform.position - previousPos) / Time.deltaTime;

        //The Player's Current Velocity Is Recorded
        previousPos = player.transform.position;
    }

    //This Function Enables The Movement Mechanic
    public void enableAmplifiedMovement()
    {
        //Send The Player To The Next Level If They Are Allowed To Leave The Level
        if (canPlayerLeave == true)
        {
            GameObject exit = GameObject.FindGameObjectWithTag("Exit");

            //Retrieves The Data From The 'cornerData.text' File, And Splits By Text Lines 
            string path = Application.streamingAssetsPath + "/cornerData.text";
            StreamReader reader = new StreamReader(path);
            string text = reader.ReadToEnd();
            string[] textLines = text.Split('\n');

            //Changes The Recorded Distance Between The Player And The Exit Object
            textLines[6] = "" + (exit.transform.position.x - player.position.x) + ",0," + (exit.transform.position.z - player.position.z) + "\n";

            //Closes The Text File
            reader.Close();

            //Records Edited Data Into The File 'cornerData.text'
            path = Application.streamingAssetsPath + "/cornerData.text";
            for (int i = 0; i < textLines.Length; i++)
            {
                if (i == 0)
                {
                    File.WriteAllText(path, "" + textLines[i] + "\n");
                }
                else
                {
                    File.AppendAllText(path, "" + textLines[i] + "\n");
                }
            }

            //Reference To The ReadyNextLevel Class
            ready = exit.GetComponent<ReadyNextLevel>();

            //Loads Next Level
            SceneManager.LoadScene(ready.nextLevel);
        }else{
            //Calculates The Distance Betwen The Player And The Movement Ring
            distanceBetween = Vector3.Distance( new Vector3(player.position.x, 0, player.position.z), 
                                                new Vector3(transform.parent.transform.position.x, 0, transform.parent.transform.position.z));

            //Enables The Movement Mechanic If The Distance Calculated Is Less Than The Radius,
            //And If The Movement Mechanic Is Currently Disabled
            if (distanceBetween <= radius && ovr.canAmplify == false && GetComponent<Renderer>().material.color.a >= 0.45f)
            {
                //Stores The Position Difference Between The Player And The Movement Ring
                posDiff = new Vector3(  transform.parent.transform.position.x - player.position.x,
                                        0,
                                        transform.parent.transform.position.z - player.position.z);
                ovr.changeOriginPos = true;
                ovr.canAmplify = true;
                movementEnabled = true;
            }
        }
    }

    //This Function Disables The Movement Mechanic
    public void disableAmplifiedMovement()
    {
        //Disables The Movement Mechanic If It Is Currently Enabled
        if (ovr.canAmplify == true)
        {
            ovr.changeOriginPos = true;
            ovr.canAmplify = false;
            movementEnabled = false;
        }
    }
}
