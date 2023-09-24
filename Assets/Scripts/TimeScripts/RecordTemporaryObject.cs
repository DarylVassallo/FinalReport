using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Stores The Lists Used For A Temporary Object
public class RecordTemporaryObject : MonoBehaviour
{
    //These Lists Are Used To Record The Position, Rotation, Velocity, And Angular Velocity, As Well As The Active Values Of The Gravity, And Collision Of The Temporary Object
    public List<Quaternion> objectRotation = new List<Quaternion>();
    public List<Vector3> objectPosition = new List<Vector3>();

    public List<Vector3> objectVelocity = new List<Vector3>();
    public List<Vector3> objectAngularVelocity = new List<Vector3>();

    public List<bool> objectGravity = new List<bool>();
    public List<bool> objectCollision = new List<bool>();
}
