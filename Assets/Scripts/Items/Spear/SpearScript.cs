using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{
    private bool collisionSpear = false;


    public Character character;
    public bool GetCollisionStatus()
    {
        return collisionSpear;
    }

    public void SetCollisionStatus(bool value)
    {
        collisionSpear = false;
    }

    public void SetSpearActive(bool value)
    {
        GetComponent<Collider>().enabled = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            collisionSpear = true;
            character.TargetFixed = true;
        }
    }


    public void TriggerExit()
    {
        Debug.Log("No way!!");
        collisionSpear = false;
        character.ThrowingSpear = false;
        //character.TargetFixed = false;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            
        }
    }*/
}
