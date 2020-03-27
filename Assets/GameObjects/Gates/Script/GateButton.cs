using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    //private Animator animator;

    private bool active;
    private bool pressed;
    [SerializeField]
    private bool alive;

    [SerializeField]
    private int order = 1;

    [SerializeField]
    private Gates gates;


    public bool IsPressed()
    {
        return pressed;
    }

    void Start()
    {
        //animator = GetComponent<Animator>();
        pressed = false;
        active = false;
        alive = true;
    }

    public void Die()
    {
        alive = false;
    }

    public void Reset()
    {
        pressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive)
        {
            var characterScript = other.gameObject.GetComponent<Character>();
            if (characterScript != null)
            {
                Debug.Log("Any!");
                if (!active)
                {
                    active = true;
                    gates.Check(order);
                    //Check();

                    //animator.SetBool("Active", active);
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (alive)
        {
            var characterScript = other.gameObject.GetComponent<Character>();
            if (characterScript != null)
            {
                Debug.Log("How!");
                if (active)
                {
                    active = false;
                    //animator.SetBool("Active", active);
                }
            }
        }
       
    }
}
