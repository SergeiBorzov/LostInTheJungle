using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    public Character character;

    private Ledge grabbedLedge;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Ledge"))
        {
            character.isGrabbingLedge = true;

            grabbedLedge = other.gameObject.GetComponent<Ledge>();

            Vector3 offset = -transform.position + grabbedLedge.transform.position;            

            character.AdjustPosition(offset, grabbedLedge.ledgeOffset, grabbedLedge.transform);

            Debug.Log("HEY!");
            //character.transform.
            //character.transform.localPosition += offset;
            //character.transform.parent = null;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ledge"))
        {
            grabbedLedge = null;
            //character.isGrabbingLedge = false;
        }
    }
}
