using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.XR;
using Node = UnityEngine.XR.XRNode;

//This Class Creates Portals to Different Universes, And Moves Them To the Correct Position And Rotation
public class CreatePortal : MonoBehaviour
{
    private InteractableUnityEventWrapper inter;

    [Header("RAY INTERACTORS")]
    //Left And Right Ray Interactors
    public RayInteractor rightInteractor;
    public RayInteractor leftInteractor;

    [Header("PORTALS")]
    //Portal Object Types
    public GameObject portal1;
    public GameObject portal2;

    private GameObject rightPortal;
    private GameObject leftPortal;

    [Header("PLAYER")]
    //Player Camera
    private Transform player;

    //Reference To The PlayerUniverseTracker Class
    private PlayerUniverseTracker playerTracker;

    [Header("OVRCAMERARIG")]
    //Reference To The OVRCameraRig Class
    private OVRCameraRig ovr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        playerTracker = GameObject.FindWithTag("MainPlayerBody").GetComponent<PlayerUniverseTracker>();

        ovr = GameObject.FindWithTag("OVRCameraRig").GetComponent<OVRCameraRig>();

        inter = GetComponent<InteractableUnityEventWrapper>();
    }

    //This Function Creates A New Portal
    public void AddPortal()
    {
        //Gets All Existing Portals In The Level
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");

        //Creates A Portal To The Next Universe In The Order:
        //If The Right Hand Ray Interactor Was Used
        if (inter.selectedInteractorView.Equals(rightInteractor))
        {

            //Creates A Portal to The Next Universe In The Order:
            //If The Distance Between The Position Of The Potential Portal, And The Player Is Greater Or Equal To One 
            if (Vector3.Distance(new Vector3(rightInteractor.CollisionInfo.Value.Point.x,
                                    player.position.y,
                                    rightInteractor.CollisionInfo.Value.Point.z), player.position) >= 0.5f)
            {

                //Checks If Any Of The Existing Portals Lead To The Next Universe In The Order 
                for (int i = 0; i < portals.Length; i++)
                {
                    ChangeUniverse changeUniverse = portals[i].GetComponent<ChangeUniverse>();

                    if (changeUniverse.hiddenUniverse == (playerTracker.currentUniverse + 1) || (changeUniverse.hiddenUniverse == 1 && playerTracker.currentUniverse == 3))
                    {
                        rightPortal = portals[i];
                    }
                }

                //Creates A New Portal If The Required Portal Does Not Already Exist
                if (rightPortal == null)
                {

                    //The Portal Is Created, And Positioned Where The Player Pointed On The Floor, Facing The Player
                    GameObject newPortal = Instantiate(portal1, new Vector3(rightInteractor.CollisionInfo.Value.Point.x,
                                                                        rightInteractor.CollisionInfo.Value.Point.y,
                                                                        rightInteractor.CollisionInfo.Value.Point.z), Quaternion.identity);
                    newPortal.transform.position = new Vector3(newPortal.transform.position.x,
                                                                newPortal.transform.position.y + (newPortal.transform.localScale.y / 2),
                                                                newPortal.transform.position.z);
                    newPortal.transform.LookAt(new Vector3(player.position.x, newPortal.transform.position.y, player.position.z));

                    if (ovr.canAmplify == true)
                    {
                        Vector3 centreEyePosition = Vector3.zero;
                        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centreEyePosition))
                        {
                            newPortal.transform.LookAt(new Vector3(player.position.x + ((centreEyePosition.x * ovr.amplifier) - centreEyePosition.x),
                                                                    newPortal.transform.position.y,
                                                                    player.position.z + ((centreEyePosition.z * ovr.amplifier) - centreEyePosition.z)));
                        }
                    }

                    //The Portal Is Set To Show The Required Universe
                    ChangeUniverse change = newPortal.GetComponent<ChangeUniverse>();

                    int portalUniverse;

                    if (playerTracker.currentUniverse == 3)
                    {
                        portalUniverse = 1;
                    }
                    else
                    {
                        portalUniverse = (playerTracker.currentUniverse + 1);
                    }

                    change.setUp(portalUniverse);
                }
                else
                {

                    //The Required Portal Found In The Level Is Positioned Where The Player Pointed On The Floor, Facing The Player
                    rightPortal.transform.position = new Vector3(rightInteractor.CollisionInfo.Value.Point.x,
                                                                    rightInteractor.CollisionInfo.Value.Point.y,
                                                                    rightInteractor.CollisionInfo.Value.Point.z);
                    rightPortal.transform.position = new Vector3(rightPortal.transform.position.x,
                                                                    rightPortal.transform.position.y + (rightPortal.transform.localScale.y / 2),
                                                                    rightPortal.transform.position.z);
                    rightPortal.transform.LookAt(new Vector3(player.position.x, rightPortal.transform.position.y, player.position.z));

                    if (ovr.canAmplify == true)
                    {
                        Vector3 centreEyePosition = Vector3.zero;
                        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centreEyePosition))
                        {
                            rightPortal.transform.LookAt(new Vector3(player.position.x,
                                                                        rightPortal.transform.position.y,
                                                                        player.position.z));
                        }
                    }
                }

                rightPortal = null;
            }
        }

        //Creates A Portal To The Previous Universe In The Order:
        //If The Left Hand Ray Interactor Was Used
        if (inter.selectedInteractorView.Equals(leftInteractor))
        {

            //Creates A Portal to The Previous Universe In The Order:
            //If The Distance Between The Position Of The Potential Portal, And The Player Is Greater Or Equal To One 
            if (Vector3.Distance(new Vector3(leftInteractor.CollisionInfo.Value.Point.x,
                                    player.position.y,
                                    leftInteractor.CollisionInfo.Value.Point.z), player.position) >= 0.5f)
            {

                //Checks If Any Of The Existing Portals Lead To The Previous Universe In The Order 
                for (int i = 0; i < portals.Length; i++)
                {
                    ChangeUniverse changeUniverse = portals[i].GetComponent<ChangeUniverse>();

                    if (changeUniverse.hiddenUniverse == (playerTracker.currentUniverse - 1) || (changeUniverse.hiddenUniverse == 3 && playerTracker.currentUniverse == 1))
                    {
                        leftPortal = portals[i];
                    }
                }

                //Creates A New Portal If The Required Portal Does Not Already Exist
                if (leftPortal == null)
                {

                    //The Portal Is Created, And Positioned Where The Player Pointed On The Floor, Facing The Player
                    GameObject newPortal = Instantiate(portal2, new Vector3(leftInteractor.CollisionInfo.Value.Point.x,
                                                                            leftInteractor.CollisionInfo.Value.Point.y,
                                                                            leftInteractor.CollisionInfo.Value.Point.z), Quaternion.identity);
                    newPortal.transform.position = new Vector3(newPortal.transform.position.x,
                                                                newPortal.transform.position.y + (newPortal.transform.localScale.y / 2),
                                                                newPortal.transform.position.z);
                    newPortal.transform.LookAt(new Vector3(player.transform.position.x, newPortal.transform.position.y, player.transform.position.z));

                    if (ovr.canAmplify == true)
                    {
                        Vector3 centreEyePosition = Vector3.zero;
                        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centreEyePosition))
                        {
                            newPortal.transform.LookAt(new Vector3(player.transform.position.x + ((centreEyePosition.x * ovr.amplifier) - centreEyePosition.x),
                                                                    newPortal.transform.position.y,
                                                                    player.transform.position.z + ((centreEyePosition.z * ovr.amplifier) - centreEyePosition.z)));
                        }
                    }

                    //The Portal Is Set To Show The Required Universe
                    ChangeUniverse change = newPortal.GetComponent<ChangeUniverse>();

                    int portalUniverse;
                    if (playerTracker.currentUniverse == 1)
                    {
                        portalUniverse = 3;
                    }
                    else
                    {
                        portalUniverse = (playerTracker.currentUniverse - 1);
                    }

                    change.setUp(portalUniverse);
                }
                else
                {

                    //The Required Portal Found In The Level Is Positioned Where The Player Pointed On The Floor, Facing The Player
                    leftPortal.transform.position = new Vector3(leftInteractor.CollisionInfo.Value.Point.x,
                                                                leftInteractor.CollisionInfo.Value.Point.y,
                                                                leftInteractor.CollisionInfo.Value.Point.z);
                    leftPortal.transform.position = new Vector3(leftPortal.transform.position.x,
                                                                leftPortal.transform.position.y + (leftPortal.transform.localScale.y / 2),
                                                                leftPortal.transform.position.z);
                    leftPortal.transform.LookAt(new Vector3(player.transform.position.x, leftPortal.transform.position.y, player.transform.position.z));

                    if (ovr.canAmplify == true)
                    {
                        Vector3 centreEyePosition = Vector3.zero;
                        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centreEyePosition))
                        {
                            leftPortal.transform.LookAt(new Vector3(player.transform.position.x + ((centreEyePosition.x * ovr.amplifier) - centreEyePosition.x),
                                                                    leftPortal.transform.position.y,
                                                                    player.transform.position.z + ((centreEyePosition.z * ovr.amplifier) - centreEyePosition.z)));
                        }
                    }
                }

                leftPortal = null;
            }
        }
    }
}
