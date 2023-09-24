using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Is Used To Identify An Object That Is Interactable And Within Grabbing Distance
public class PastGrabObject : MonoBehaviour
{
    //The Interactable Object Currently Grabbed By The Hand
    public GameObject grabbedObject;

    //List Of All Interactable Objects
    public List<GameObject> interactableObjects;

    //Distance Between An Interactable Object And The Hand
    private float distance;

    //The Minimum Distance Between The Hand And An Interactable Objcet
    private float minDistance;

    public Transform handBase;
    // Start is called before the first frame update
    void Start()
    {
        //Stores All The Interactable Objects Into The List
        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));

    }

    // Update is called once per frame
    void Update()
    {
        //This Is The Minimum Distance Between The Hand And An Interactable Object For It To Be Grabbed
        minDistance = 0.9f;

        //Removes Any Values From The Variable
        grabbedObject = null;

        //Goes Through Every Interactable Object In The interactableObjects List
        foreach (GameObject intObject in interactableObjects)
        {
            //Debug.Log("INTERACT1: " + intObject);
            //Stores The Distance Between The Hand And The Current Interactable Object In The Variable distance
            distance = Vector3.Distance(handBase.transform.position, intObject.transform.position);
            //Debug.Log("INTERACT2: " + distance);
            //Checks If distance Is Less Than The Minimum Distance
            if (minDistance > distance)
            {
                //Debug.Log("INTERACT3: " + distance);
                //Sets The Minimum Distance Equal To distance, And Stores The Current Object In The Variable grabbedObject
                minDistance = distance;
                grabbedObject = intObject;
            }
        }
    }
}
