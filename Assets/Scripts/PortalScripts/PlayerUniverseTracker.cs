using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUniverseTracker : MonoBehaviour
{
    [Header("CURRENT UNIVERSE")]
    //Shows The Number Of The Universe The Player Is Currently In
    public int currentUniverse;

    // Start is called before the first frame update
    void Start()
    {
        //Shows That The Player Is In The First Universe
        currentUniverse = 1;
    }
}
