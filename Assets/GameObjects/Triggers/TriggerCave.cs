using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCave : MonoBehaviour
{
    [SerializeField]
    private Wall wall;

    private bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            wall.Action();
            active = true;
        }
        else
        {
            wall.Deaction();
            active = false;
        }       
    }
}
