using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        var characterScript = other.gameObject.GetComponent<Character>();

        if (characterScript != null)
        {
            characterScript.Die();
        }
    }
}
