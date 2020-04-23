using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LedgeChecker : MonoBehaviour
{
    [HideInInspector]
    public Character character;

    private Ledge grabbedLedge;


    private void Start()
    {
        character = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var ledge = other.gameObject.GetComponent<Ledge>();
        if (ledge != null)
        {
           
            if (!character.isGrabbingLedge)
            {
                character.isGrabbingLedge = true;
                character.SetAnimatorHanging();

                character.grabbedLedge = ledge;

                Vector3 offset = -transform.position + ledge.transform.position;

                character.AdjustPosition(offset, ledge.ledgeOffset, ledge.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var ledge = other.gameObject.GetComponent<Ledge>();
        if (ledge != null)
        {
            grabbedLedge = null;
        }
        
    }
}
