using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Amplifies The Size Of The Level To The Required Size
public class MagnifyMap : MonoBehaviour
{
    [Header("ADD EXISTING CORNERS")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    //List For All Interactable Objects
    private List<GameObject> interactableObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The AddExistingCorners Class
        add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Records All Existing Interactable Objects
        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));
    }

    //This Function Amplifies the Size Of The Level
    public void EnlargenMap()
    {
        //Reference To The AddExistingCorners Class
        add = GameObject.FindGameObjectWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //The Sets The Scale Of the Level, Using The XLength, And ZLength Variables:
        //If The Variable allowEnlargen Shows That The Level Can Be Enlargened
        if (add.allowEnlargen == true)
        {
            this.transform.localScale = new Vector3(add.XLength,
                                                    5,
                                                    add.ZLength);
        }

        //Records All Existing Interactable Objects
        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

        //Reset The Position And Rotation Of All Interactable Objects
        foreach (GameObject intObject in interactableObjects)
        {
            intObject.GetComponent<ResetPosition>().resetPositionRotation();
        }
    }
}
