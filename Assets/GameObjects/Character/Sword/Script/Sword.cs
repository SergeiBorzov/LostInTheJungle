using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Character characterScript;
    Rigidbody rb;

    public bool canDamage = false;
    private void Start()
    {
        characterScript = GetComponentInParent<Character>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fracture script = collision.gameObject.GetComponent<Fracture>();

        
        if (script != null && (characterScript.isFight == true || characterScript.isFightEnd == true) )
        {
            script.Action();
            return;
        }

        /*Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (canDamage)
            {
                enemy.TakeDamage();
                canDamage = false;
            }
            return;
        }*/
    }
}
