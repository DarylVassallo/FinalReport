using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Stores The Lists Used For An Interactable Object
public class RecordInteractableObject : MonoBehaviour
{
    //These Lists Are Used To Record The Position, Rotation, Velocity, And Angular Velocity Of The Interactable Object
    public List<Quaternion> objectRotation = new List<Quaternion>();
    public List<Vector3> objectPosition = new List<Vector3>();

    public List<Vector3> objectVelocity = new List<Vector3>();
    public List<Vector3> objectAngularVelocity = new List<Vector3>();
}
