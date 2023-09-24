using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Changes The Color Of The Material Depending On If The Button Has Been Activated
public class ChangeColorUponActivation : MonoBehaviour
{
    [Header("BUTTON ACTIVATION")]
    //Reference To The ButtonActivation Class
    public ButtonActivation button;

    [Header("RENDERER")]
    //Reference To The Renderer Material
    private Renderer indicatorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The Renderer Material
        indicatorRenderer = GetComponent<Renderer>();

        //Set's The Cube's Material Color To Red
        indicatorRenderer.material.color = new Color(button.brightValue, 0, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks If This Object Is An Interactable Object
        if(this.gameObject.tag == "InteractableObject")
        {
            //Checks If The Button Was Activated
            if (button.activated)
            {
                //Sets The Cube's Material Color To Green
                indicatorRenderer.material.color = new Color(0, button.brightValue, 0, 255);
            }else{
                //Sets The Cube's Material Color To Red
                indicatorRenderer.material.color = new Color(button.brightValue, 0, 0, 255);
            }
        }        
    }
}
