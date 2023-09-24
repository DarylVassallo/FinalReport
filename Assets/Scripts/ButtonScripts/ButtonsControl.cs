using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Checks If All Existing Buttons Have Been Activated
public class ButtonsControl : MonoBehaviour
{
    public GameObject exit;
    private ButtonActivation multiButton;
    private int buttonActiveCounter;
    // Start is called before the first frame update
    void Start()
    {
        exit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");

        buttonActiveCounter = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            multiButton = buttons[i].GetComponent<ButtonActivation>();

            if(multiButton.activated == true)
            {
                buttonActiveCounter++;
            }
        }

        if(buttonActiveCounter == buttons.Length)
        {
            exit.SetActive(true);
        }
    }
}
