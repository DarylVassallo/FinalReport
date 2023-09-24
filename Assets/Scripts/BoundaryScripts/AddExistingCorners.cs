using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Oculus.Interaction.Input;

//This Class Sets Up the Level Walls And Allows The Level To Be Enlargened
public class AddExistingCorners : MonoBehaviour
{
    [Header("ROOM")]
    //Universe 1 And 2 Wall GameObjects
    public GameObject castleWall;
    public GameObject futureWall;

    //Level Lengths
    public float XLength;
    public float ZLength;

    //Shows If The Level Can Be Enlargened
    public bool allowEnlargen;

    public GameObject ground;

    private MagnifyMap map;

    public float minimizeSize;

    [Header("PORTAL")]
    //Reference To The LayerNumbers Class
    public LayerNumbers layer;

    //Shows If The Level Uses The Portal Mechanic
    public bool usesPortals;

    //Reference To The ChangeObjectLayer Class
    private ChangeObjectLayer wallLayer;

    [Header("OVRCAMERARIG")]
    //Reference To The OVRCameraRig Class
    private OVRCameraRig ovr;

    [Header("RESET")]
    //Shows If The Reset Mechanic Is Being Used
    public bool isResetActivated;

    [Header("NARRATOR SUBTITLES")]
    //Narrator SubTitles Text
    public Transform audioText;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The OVRCameraRig Class
        ovr = GameObject.FindWithTag("OVRCameraRig").GetComponent<OVRCameraRig>();

        //Reference To The MagnifyMap Class
        map = ground.GetComponent<MagnifyMap>();
        
        //Shows That The Level Cannot Be Enlargened
        allowEnlargen = false;

        //Retrieves The Data From The 'cornerData.text' File, And Splits By Text Lines 
        string path = Application.streamingAssetsPath + "/cornerData.text";
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        string[] textLines = text.Split('\n');

        //Stores The Position Data From 'cornerData.text' Into corners[]
        Vector3[] corners = new Vector3[4];
        
        for (int i = 0; i < 4; i++)
        {
            string[] linePos = textLines[i].Split(char.Parse(","));
            corners[i] = new Vector3(float.Parse(linePos[0]), float.Parse(linePos[1]), float.Parse(linePos[2]));
        }

        //Stores The Amplification Value From 'cornerData.text'
        ovr.amplifier = float.Parse(textLines[4]);

        //Rotates the Player To Face The Correct Direction, Using The Rotation Value From 'cornerData.text'
        GameObject.FindWithTag("OVRCameraRig").transform.Rotate(0, float.Parse(textLines[5]), 0);

        //Creates And Sets The Position Of Each Wall
        //If The Level Does Not Use The Portal Mechanic, Then One Set Of Walls Are Created And Placed, Using The Positions Of The Corners
        //If The Level Uses The Portal Mechanic, Then Two Sets Of Walls Are Created And Placed In The Same Place,
        //Using The Positions Of The Corners

        //If The Level Uses The Portal Mechanic, Then Two Sets Of Walls Will Be Created, Otherwise One Set Will Be Created
        int wallAmount = 0;
        if(usesPortals == true)
        {
            wallAmount = 2;
        }else{
            wallAmount = 1;
        }

        //Creates The Sets Of Walls And Positions Them
        for (int i = 1; i <= wallAmount; i++)
        {
            //Checks Which Type Of Wall Will Be Created For The Current Set
            GameObject wall = castleWall;
            if (i == 1)
            {
                wall = futureWall;
            }else
            if (i == 2)
            {
                wall = castleWall;
            }

            //Sets The Universe the Wall Exist In:
            //If The Level Uses The Portal Mechanic
            if (usesPortals == true) 
            {
                wallLayer = wall.GetComponent<ChangeObjectLayer>();
                wallLayer.currentUniverse = i;
            }

            //Creates And Sets The Position Of Each Wall In The Current Set
            for (int j = 0; j < 4; j++)
            {
                GameObject newWall;
                if (j != 3)
                {
                    //Sets The Wall Between Two Corners, And Changes Its Scale To Fill The Space Between Them
                    newWall = Instantiate(wall, new Vector3(((corners[j].x + corners[j + 1].x) / 2) * minimizeSize,
                                                                        0,
                                                                       ((corners[j].z + corners[j + 1].z) / 2) * minimizeSize), Quaternion.identity);

                    newWall.transform.localScale = new Vector3(Vector3.Distance(corners[j], corners[j + 1]),
                                                                                newWall.gameObject.transform.localScale.y,
                                                                                newWall.gameObject.transform.localScale.z);
                }else{
                    //Sets The Wall Between Two Corners, And Changes Its Scale To Fill The Space Between Them
                    newWall = Instantiate(wall, new Vector3(((corners[j].x + corners[0].x) / 2) * minimizeSize,
                                                                        0,
                                                                       ((corners[j].z + corners[0].z) / 2) * minimizeSize), Quaternion.identity);

                    newWall.transform.localScale = new Vector3(Vector3.Distance(corners[j], corners[0]),
                                                                                newWall.gameObject.transform.localScale.y,
                                                                                newWall.gameObject.transform.localScale.z);

                    //Positions The Narrator's Subtitles In Front Of The Wall, In Its Centre
                    audioText.position = new Vector3(newWall.transform.position.x * 0.9f, 3, newWall.transform.position.z * 0.9f);
                    audioText.LookAt(new Vector3(this.transform.position.x, audioText.position.y, this.transform.position.z));
                    audioText.Rotate(0, 180, 0);
                }

                //Sets The Wall To Face The Centre Of The Room
                newWall.transform.LookAt(new Vector3(this.transform.position.x, newWall.transform.position.y, this.transform.position.z));

                //Corrects The Wall's Angle To Prevent It From Being Slanted
                int yRot;
                if (newWall.transform.eulerAngles.y >= -315 && newWall.transform.eulerAngles.y <= -225)
                {
                    yRot = -270;
                }else
                if (newWall.transform.eulerAngles.y > -225 && newWall.transform.eulerAngles.y <= -135)
                {
                    yRot = -180;
                }else
                if (newWall.transform.eulerAngles.y > -135 && newWall.transform.eulerAngles.y <= -45)
                {
                    yRot = -90;
                }else
                if (newWall.transform.eulerAngles.y > -45 && newWall.transform.eulerAngles.y <= 45)
                {
                    yRot = 0;
                }else
                if (newWall.transform.eulerAngles.y > 45 && newWall.transform.eulerAngles.y <= 135)
                {
                    yRot = 90;
                }else
                if (newWall.transform.eulerAngles.y > 135 && newWall.transform.eulerAngles.y <= 225)
                {
                    yRot = 180;
                }
                else
                if (newWall.transform.eulerAngles.y > 225 && newWall.transform.eulerAngles.y <= 315)
                {
                    yRot = 270;
                }else{
                    yRot = 0;
                }

                newWall.transform.rotation = Quaternion.Euler(0, yRot, 0);
            }

            //Calculates The X And Z Lengths Of The Level
            //The Lengths Are Reduced To Prevent The Need Of The Player Moving Very Close To The Play Area's Boundaries
            if (corners[0].x == corners[1].x)
            {
                XLength = Vector3.Distance(corners[1], corners[2]) * minimizeSize;
                ZLength = Vector3.Distance(corners[1], corners[0]) * minimizeSize;
            }else{
                XLength = Vector3.Distance(corners[1], corners[0]) * minimizeSize;
                ZLength = Vector3.Distance(corners[1], corners[2]) * minimizeSize;
            }

            //Shows That The Level Can Be Enlargened
            allowEnlargen = true;

            //Creates The Roof And Bottom Of The Level
            //Sets Their Positions to Be Over And Under The Level Platforms
            //Sets Their Scale To fill To Cover The Top And Bottom Of The Level Completely
            GameObject roof = Instantiate(wall, new Vector3(0, 7, 0), Quaternion.identity);
            roof.transform.localScale = new Vector3(XLength, 0.01f, ZLength);

            GameObject bottom = Instantiate(wall, new Vector3(0, -7, 0), Quaternion.identity);
            bottom.transform.localScale = new Vector3(XLength, 0.01f, ZLength);
        }

        //The Level Is Set to The Origin Point Of The Environment
        ground.transform.position = new Vector3(0,0,0);

        //OVRCameraRig Is Enabled, And The Level Is Enlargened
        ovr.enabled = true;
        map.EnlargenMap();
    }
}
