using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    public Character character;

    private Ledge grabbedLedge;

    private void Start()
    {
        character = GetComponentInParent<Character>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ledge"))
        {
           
            if (!character.isGrabbingLedge)
            {
                character.isGrabbingLedge = true;
                character.SetAnimatorHanging();

                grabbedLedge = other.gameObject.GetComponent<Ledge>();
                character.grabbedLedge = grabbedLedge;

                Vector3 offset = -transform.position + grabbedLedge.transform.position;

                character.AdjustPosition(offset, grabbedLedge.ledgeOffset, grabbedLedge.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.gameObject.CompareTag("Ledge"))
        {
            grabbedLedge = null;
        }
        */
    }
}
