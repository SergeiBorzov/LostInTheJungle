using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Character characterScript;

    private void Start()
    {
        characterScript = GetComponentInParent<Character>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fracture script = collision.gameObject.GetComponent<Fracture>();
        
        if (script != null && (characterScript.isFight == true || characterScript.isFightEnd == true) )
        {
            script.Action();
        }
    }
}
