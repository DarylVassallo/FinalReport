using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//This Class Saves The Corner Data, And The Amplification Value. It Also Sends The Player To The Next Level
public class BoundaryPositions : MonoBehaviour
{
    [Header("PLAYER")]
    //Player Camera
    private Transform player;

    [Header("OVRCAMERARIG")]
    //Reference To The OVRCameraRig Class
    private OVRCameraRig ovr;

    [Header("BOUNDARY VARIABLES")]
    //Shows If The Player Can Confirm The Boundary Position And Size
    public bool boundaryMode;

    //The Minimum Distance The Player  Has To Be In To Confirm The Boundary Position And Size 
    public float radius;

    //Minimum Level Length
    [SerializeField]
    private float levelLength;

    private bool hasSavedData;
    // Start is called before the first frame update
    void Start()
    {
        //Reference To The Player's Camera
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //Reference To The OVRCameraRig Class
        ovr = GameObject.FindGameObjectWithTag("OVRCameraRig").GetComponent<OVRCameraRig>();

        //Shows That The Player Cannot Confirm The Boundary Position And Size
        ovr.canAmplify = false;

        //Shows That The Data Does Not Need To Be Saved Again
        hasSavedData = false;

        //Sets The Position And Scale of The Object
        transform.position = new Vector3(0, player.position.y - 0.5f, 0);
        transform.localScale = new Vector3(radius * 2, transform.localScale.y, radius * 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Gets All Existing Boundary Corners In The Level
        GameObject[] corners = GameObject.FindGameObjectsWithTag("Corner");

        //Sets This Object In The Center Of The Room, Created By the Boundaries
        transform.position = new Vector3(   (corners[0].transform.position.x + corners[1].transform.position.x + corners[2].transform.position.x + corners[3].transform.position.x) / 4,
                                            player.position.y - 0.5f,
                                            (corners[0].transform.position.z + corners[1].transform.position.z + corners[2].transform.position.z + corners[3].transform.position.z) / 4);

        //Shows That The Player Can Confirm The Boundary Position And Size If the Player Is Within The Radius Of The Object In The X And Z Axis
        if ( player.position.x >= (transform.position.x - radius) && player.position.x <= (transform.position.x + radius) &&
            player.position.z >= (transform.position.z - radius) && player.position.z <= (transform.position.z + radius))
        {
            boundaryMode = true;
        }
    }

    //This Function Saves The Corner Positions And The Amplification Value
    public void saveBoundaryPoints()
    {
        //Checks If The Data Has Already Been Saved
        if (hasSavedData == false)
        {
            //Shows That The Data Does Not Need To Be Saved Again
            hasSavedData = true;

            //Gets All Existing Boundary Corners In The Level
            GameObject[] corners = GameObject.FindGameObjectsWithTag("Corner");

            float minLength;

            //Finds The Smallest Side Of The Boundary Walls
            if (Vector3.Distance(corners[0].transform.position, corners[1].transform.position) > Vector3.Distance(corners[0].transform.position, corners[3].transform.position))
            {
                minLength = Vector3.Distance(corners[0].transform.position, corners[3].transform.position);
            }
            else
            {
                minLength = Vector3.Distance(corners[0].transform.position, corners[1].transform.position);
            }

            //Sets The Amplification Value
            ovr.amplifier = levelLength / minLength;

            //Amplifies The Position Of Each Corner, Using The Amplification Value
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i].transform.parent = this.transform;

                float dx = corners[i].transform.position.x - this.transform.position.x;
                float dz = corners[i].transform.position.z - this.transform.position.z;

                corners[i].transform.position = new Vector3(this.transform.position.x + (dx * ovr.amplifier),
                                                               corners[i].transform.position.y,
                                                                this.transform.position.z + (dz * ovr.amplifier));
                corners[i].transform.parent = null;
            }

            //Records Each Corner Position Into The File 'cornerData.text'
            string path = Application.streamingAssetsPath + "/cornerData.text";
            for (int i = 0; i < corners.Length; i++)
            {
                if (i == 0)
                {
                    File.WriteAllText(path, "" + corners[i].transform.position.x + "," + corners[i].transform.position.y + "," + corners[i].transform.position.z + "\n");
                }
                else
                {
                    File.AppendAllText(path, "" + corners[i].transform.position.x + "," + corners[i].transform.position.y + "," + corners[i].transform.position.z + "\n");
                }
            }

            //Records The Amplification Value Into The File 'cornerData.text'
            File.AppendAllText(path, "" + ovr.amplifier + "\n");

            //Records The Rotation Value Of The Player Into The File 'cornerData.text'
            File.AppendAllText(path, "" + GameObject.FindGameObjectWithTag("Level").GetComponent<BoundarySetUp>().playerRotation + "\n");

            //Records The Distance Between The BoundaryPositions Object, And The Player Into The File 'cornerData.text'
            File.AppendAllText(path, "" + (transform.position.x - player.position.x) + ",0," + (transform.position.z - player.position.z) + "\n");

            //Sends the Player to The Level 'FirstLevel'
            SceneManager.LoadScene("FirstLevel");
        }
    }
}