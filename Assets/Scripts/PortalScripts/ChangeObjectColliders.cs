using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

//This Class Checks If The Object Can Be Used To Create Portals
public class ChangeObjectColliders : MonoBehaviour
{
    //Reference To The LayerNumbers Class
    private LayerNumbers layers;

    //Reference To The CreatePortal Class
    private CreatePortal createPortal;

    //Reference To The RayInteractable Class
    private RayInteractable ray;

    // Start is called before the first frame update
    void Start()
    {
        //Reference To The LayerNumbers Class
        layers = GameObject.FindWithTag("Level").GetComponent<LayerNumbers>();

        //Reference To The CreatePortal Class
        createPortal = this.GetComponent<CreatePortal>();

        //Reference To The RayInteractable Class
        ray = this.GetComponent<RayInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        //The RayInteractable And CreatePortal Classes Are Disabled If The Object's Layer Is Not The Visible Layer
        if(this.gameObject.layer != layers.showOutsidePortal)
        {
            createPortal.enabled = false;
            ray.enabled = false;

        //Otherwise, The RayInteractable And CreatePortal Classes Are Enabled
        }else{
            createPortal.enabled = true;
            ray.enabled = true;
        }
    }
}
