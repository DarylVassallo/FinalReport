using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Activate The Button When An Interactable Object Is On Top Of It
public class ButtonActivation : MonoBehaviour
{
    [Header("BUTTON RANGE")]
    //Stores The X And Z Range Allowed For The Button To Be Activated
    [SerializeField]
    private float xzrange;

    //Stores All Existing Buttons
    private GameObject[] buttons;

    //Stores The Y Range Allowed For The Button To Be Activated
    [SerializeField]
    private float yrange;

    [Header("RENDERER")]
    //Stores The Level Of Brightness Of The Renderer
    public float brightValue;

    //References The Button's Renderer Class
    private Renderer indicatorRenderer;

    //References The Interactable Object's Renderer Class
    private Renderer interactableRenderer;

    [Header("INTOBJECTS")]
    [SerializeField]
    //The Interactable Object Required To Activate The Button
    private GameObject requiredObject;

    //Stores All The Interactable Objects Into A List
    private List<GameObject> interactableObjects;

    //Shows If The Button Has Been Activated
    public bool activated;

    [Header("EXIT")]
    //Exit GameObject
    public GameObject exit;

    [Header("ADD EXISTING CORNERS")]
    //Reference To The AddExistingCorners Class
    private AddExistingCorners add;

    // Start is called before the first frame update
    void Start()
    {
        //Stores All Existing Buttons
        buttons = GameObject.FindGameObjectsWithTag("Button");

        //Reference To The AddExistingCorners Class
        add = GameObject.FindWithTag("CornerSetter").GetComponent<AddExistingCorners>();

        //Reference To The Button's Renderer Class
        indicatorRenderer = this.gameObject.GetComponent<Renderer>();

        //Shows That The Button Is Not Activated
        activated = false;

        //Set's The Button's Material Color To Red
        indicatorRenderer.material.color = new Color(brightValue, 0, 0, 255);

        //Stores A List Of All The Interactable Objects In interactableObjects
        interactableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableObject"));
    }

    // Update is called once per frame
    void Update()
    {
        //Shows That The Button Is Not Activated
        activated = false;

        //Goes Through Every Interactable Object In interactableObjects
        foreach (GameObject intObject in interactableObjects)
        {
            //Checks If The Y Position Of The Object Is Greater Or Less Than The Button's Y Position By The Amout Of yrange, 
            //And If The X And Z Position Of The Object Is Greater Or Less Than The Button's X And Z Position By The Amout Of xzrange
            if (intObject.transform.position.x >= (transform.position.x - xzrange) && intObject.transform.position.x <= (transform.position.x + xzrange) &&
                intObject.transform.position.y >= (transform.position.y - yrange) && intObject.transform.position.y <= (transform.position.y + yrange) &&
                intObject.transform.position.z >= (transform.position.z - xzrange) && intObject.transform.position.z <= (transform.position.z + xzrange))
            {
                //Checks If The Current Interactable Object Is The Required Object
                if (intObject == requiredObject)
                {
                    //Reference To The Button's, And Interactable Object's Renderer Classes
                    interactableRenderer = intObject.GetComponent<Renderer>();
                    indicatorRenderer = this.gameObject.GetComponent<Renderer>();

                    //Checks If The Level Uses The Portal Mechanic
                    if (add.usesPortals == true)
                    {
                        //Reference To The Button's, And Interactable Object's ChangeObjectLayer Classes
                        ChangeObjectLayer interactableLayer = intObject.GetComponent<ChangeObjectLayer>();
                        ChangeObjectLayer indicatorLayer = this.gameObject.GetComponent<ChangeObjectLayer>();

                        //Checks If The Interactable Object, And The Button Exist In The Same Universe
                        if (interactableLayer.currentUniverse == indicatorLayer.currentUniverse)
                        {
                            //Shows That The Button Is Activated
                            activated = true;

                            //Set's The Button's, And Interactable Object's Material Color To Green
                            indicatorRenderer.material.color = new Color(0, brightValue, 0, 255);
                            interactableRenderer.material.color = new Color(0, brightValue, 0, 255);

                            //Enables The Exit GameObject If There Is Only One Button
                            if (buttons.Length == 1)
                            {
                                exit.SetActive(true);
                            }
                        }
                    }else{
                        //Shows That The Button Is Activated
                        activated = true;

                        //Set's The Button's Material Color To Green
                        indicatorRenderer.material.color = new Color(0, brightValue, 0, 255);
                        interactableRenderer.material.color = new Color(0, brightValue, 0, 255);

                        //Enables The Exit GameObject If There Is Only One Button
                        if (buttons.Length == 1)
                        {
                            exit.SetActive(true);
                        }
                    }
                }

            }
        }

        //Checks If The Button Has Been Activated
        if(activated == false)
        {
            //Set's The Button's Material Color To Red
            indicatorRenderer.material.color = new Color(brightValue, 0, 0, 255);

            //Set's The Required Object's Material Color To Red
            interactableRenderer = requiredObject.GetComponent<Renderer>();
            interactableRenderer.material.color = new Color(brightValue, 0, 0, 255);

            //Disables The Exit GameObject If There Is Only One Button
            if (buttons.Length == 1)
            {
                exit.SetActive(false);
            }
        }
    }
}
