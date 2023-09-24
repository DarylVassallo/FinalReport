using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

//This Class Sets The Position Of The Boundary Corners
public class BoundarySetUp : MonoBehaviour
{
    [Header("BOUNDARY OBJECTS")]
    //Opaque Corner Object
    public GameObject boundaryPoint;

    //Translucent Corner Object
    public GameObject possibleBoundaryPoint;

    //Wall Object
    public GameObject wallObject;

    //Number Of Existing Boundary Objects
    public int cornerNumber;

    //Stores The Amount Of Walls and Corners In The Level
    private GameObject[] walls;
    private GameObject[] corners;

    //The Second Corner Created
    private GameObject secondBoundary;

    [Header("BOUNDARY OBJECTS")]
    //Ray Used To Select Corner Positions
    public RayInteractor ray;

    //Translucent Corner That Is Placed Where The Player Points
    private GameObject movePossPoint;

    //Shows If The Player Is Pointing At The Floor
    private bool isHovering;

    [Header("BOUNDARY CENTER")]
    //Boundary Center Object
    public GameObject boundaryCenterPos;

    //Center Position Of The Boundary In The X, And Z Axis
    private float averagePosX;
    private float averagePosZ;

    //Shows Rotation Applied To Make The Player Face The Correct Direction
    [HideInInspector]
    public float playerRotation;

    // Start is called before the first frame update
    void Start()
    {
        //Boundary Center Is Deactivated
        boundaryCenterPos.SetActive(false);

        cornerNumber = 0;
        movePossPoint = Instantiate(possibleBoundaryPoint, new Vector3(0, 0, 0), Quaternion.identity);
        movePossPoint.tag = "Untagged";

        walls = new GameObject[4];
        corners = new GameObject[4];

        isHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets cornerNumber To The Number Of Existing Corners:
        //If The Player Has Not Set The Position Of The Two Diagonally Opposite Corners
        if(cornerNumber != 999)
        {
            cornerNumber = GameObject.FindGameObjectsWithTag("Corner").Length;
        }else

        //Deactivates The Boundary Center, And Sets cornerNumber To Zero:
        //If There Are No Existing Corners Or Walls
        if(GameObject.FindGameObjectsWithTag("Corner").Length == 0 && GameObject.FindGameObjectsWithTag("Wall").Length == 0)
        {
            cornerNumber = 0;
            boundaryCenterPos.SetActive(false);
        }

        //Allows movePossPoint To Be Moved:
        //If There Are Four Or Less Existing Corners, And the Player Is Pointing Towards The Floor
        if (cornerNumber < 5)
        {
            if (isHovering == true)
            {
                if (ray.CollisionInfo.Value.Point != null)
                {
                    //Moves movePossPoint, Using The Position Of Where the Ray Is Colliding with The Floor
                    movePossPoint.transform.position = new Vector3(ray.CollisionInfo.Value.Point.x,
                                                                    ray.CollisionInfo.Value.Point.y,
                                                                            ray.CollisionInfo.Value.Point.z);
                    movePossPoint.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
                }
            } 
        }

        //Sets The Position Of the Corners And Walls, As Well As Each Wall's Size:
        //If There Are Four Existing Corners
        if (cornerNumber == 4)
        {
            //The Corner Positions Are Set, Using The Position Of The First Corner, And The Position Of movePossPoint,
            //To Calculate The Position Of The Remaining Corners, And To Form A Quadilateral Area
            corners[0].transform.position = new Vector3(corners[0].transform.position.x, movePossPoint.transform.position.y, corners[0].transform.position.z);
            corners[1].transform.position = new Vector3(corners[0].transform.position.x, corners[0].transform.position.y, movePossPoint.transform.position.z);
            corners[2].transform.position = movePossPoint.transform.position;
            corners[3].transform.position = new Vector3(movePossPoint.transform.position.x, movePossPoint.transform.position.y, corners[0].transform.position.z);

            //Calculates The Center Position Between The Corners On The X And Z Axis
            averagePosX = ( corners[0].transform.position.x + corners[1].transform.position.x +
                            corners[2].transform.position.x + corners[3].transform.position.x) / 4;

            averagePosZ = ( corners[0].transform.position.z + corners[1].transform.position.z +
                            corners[2].transform.position.z + corners[3].transform.position.z) / 4;

            //Sets The Position And Scale Of Each Wall To Be Between Two Corners
            for (int i = 0; i < 4; i++)
            {
                if (i != 3){
                    walls[i].transform.position = new Vector3((corners[i].transform.position.x + corners[i + 1].transform.position.x) / 2,
                                                               corners[i].transform.position.y,
                                                              (corners[i].transform.position.z + corners[i + 1].transform.position.z) / 2);

                    walls[i].transform.localScale = new Vector3(Vector3.Distance(corners[i].transform.position, corners[i + 1].transform.position),
                                                                walls[i].gameObject.transform.localScale.y,
                                                                walls[i].gameObject.transform.localScale.z);
                }else{
                    walls[i].transform.position = new Vector3((corners[i].transform.position.x + corners[0].transform.position.x) / 2,
                                                               corners[i].transform.position.y,
                                                              (corners[i].transform.position.z + corners[0].transform.position.z) / 2);

                    walls[i].transform.localScale = new Vector3(Vector3.Distance(corners[i].transform.position, corners[0].transform.position),
                                                                        walls[i].gameObject.transform.localScale.y,
                                                                        walls[i].gameObject.transform.localScale.z);
                }

                //The Wall Is Rotated To Face The Center Of The Area
                walls[i].transform.LookAt(new Vector3(averagePosX, walls[i].transform.position.y, averagePosZ));
            }
        }
    }

    //This Function Shows If The Player Is Pointing Towards The Floor
    private void Hovering()
    {
        isHovering = true;
    }

    //This Function Shows If The Player Is Not Pointing Towards The Floor
    private void notHovering()
    {
        isHovering = false;
    }

    //This Function Adds A Corner Where The Player Was Pointing On The Floor 
    public void AddBoundaryPoint()
    {
        //Creates Four Walls And Corners. The Corner Positions Are Set Using The Position Of The First Corner, And The Position Of movePossPoint,
        //To Calculate The Position Of The Remaining Corners, And To Form A Quadilateral Area. The Position And Scale Of Each Wall Are Set To Be
        //Between Two Corners. This Happens:
        //If There Are No Existing Corners In The Level
        if (cornerNumber == 0)
        {
            corners[0] = Instantiate(boundaryPoint, new Vector3(ray.CollisionInfo.Value.Point.x, ray.CollisionInfo.Value.Point.y + 3.5f, ray.CollisionInfo.Value.Point.z), Quaternion.identity);
            corners[1] = Instantiate(possibleBoundaryPoint, new Vector3(corners[0].transform.position.x, corners[0].transform.position.y, movePossPoint.transform.position.z), Quaternion.identity);
            corners[2] = Instantiate(possibleBoundaryPoint, movePossPoint.transform.position, Quaternion.identity);
            corners[3] = Instantiate(possibleBoundaryPoint, new Vector3(movePossPoint.transform.position.x, movePossPoint.transform.position.y, corners[0].transform.position.z), Quaternion.identity);

            walls[0] = Instantiate(wallObject, corners[0].transform.position, Quaternion.identity);
            walls[1] = Instantiate(wallObject, new Vector3(corners[0].transform.position.x, corners[0].transform.position.y, movePossPoint.transform.position.z), Quaternion.identity);
            walls[2] = Instantiate(wallObject, movePossPoint.transform.position, Quaternion.identity);
            walls[3] = Instantiate(wallObject, new Vector3(movePossPoint.transform.position.x, movePossPoint.transform.position.y, corners[0].transform.position.z), Quaternion.identity);
        }else{
            //The Second Corner's Position Is Set To Where The Player Is Pointing On The Floor

            //The Colliders Of The Walls Are Enabled
            List<GameObject> walls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wall"));
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<Collider>().enabled = true;
            }

            //cornerNumber Is Set To 999, Showing That The Second Corner Has Been Placed
            cornerNumber = 999;

            //The Center Position Of The Boundary Is Calculated, On The X And Z Axis
            averagePosX = ( walls[0].transform.position.x + walls[1].transform.position.x +
                            walls[2].transform.position.x + walls[3].transform.position.x) / 4;

            averagePosZ = ( walls[0].transform.position.z + walls[1].transform.position.z +
                            walls[2].transform.position.z + walls[3].transform.position.z) / 4;

            //The Boundary Center Is Activated, And It's Position Is Set To The Center Of The Boundary
            boundaryCenterPos.SetActive(true);
            boundaryCenterPos.transform.position = new Vector3(averagePosX, 1.5f, averagePosZ);
        }
    }
}
