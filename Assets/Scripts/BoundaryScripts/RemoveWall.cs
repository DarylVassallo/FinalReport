using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Removes All Corners And Walls From The Level
public class RemoveWall : MonoBehaviour
{
    public void Remove()
    {
        List<GameObject> walls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wall"));
        walls.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Corner")));

        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
    }
}
