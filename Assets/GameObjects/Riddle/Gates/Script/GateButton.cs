using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    private Animator animator;
    private Material mat;
    private float lightUpTime = 2.0f;
    private float timer = 1.0f;

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
        animator = GetComponent<Animator>();
        pressed = false;
        active = false;
        alive = true;
        mat = GetComponent<Renderer>().material;
        timer = lightUpTime;
    }

    public void Die()
    {
        alive = false;
    }

    public void Reset()
    {
        pressed = false;
        animator.SetBool("Pressed", pressed);
        if (!active)
        {
            StopCoroutine("LightUp");
            StartCoroutine("LightDown");
        }
    }

    IEnumerator LightUp()
    {
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.0f;
            }

            mat.SetColor("_EmissionColor", Color.yellow*((lightUpTime - timer) / lightUpTime));
            yield return null;
        }
    }

    IEnumerator LightDown()
    {
        while (timer < lightUpTime)
        {
            timer += Time.deltaTime;
            if (timer > lightUpTime)
            {
                timer = lightUpTime;
            }

            mat.SetColor("_EmissionColor", Color.yellow * ((lightUpTime - timer) / lightUpTime));
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive && !pressed)
        {
            var characterScript = other.gameObject.GetComponent<Character>();
            if (characterScript != null)
            {
                if (!active)
                {
                    active = true;
                    StopCoroutine("LightDown");
                    StartCoroutine("LightUp");
                    if (gates.Check(order))
                    {
                        pressed = true;
                        animator.SetBool("Pressed", pressed);
                    };
                    animator.SetBool("Active", active);
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
                if (active)
                {
                    active = false;
                    if (!pressed)
                    {
                        StopCoroutine("LightUp");
                        StartCoroutine("LightDown");
                    }
                   
                    animator.SetBool("Active", active);
                }
            }
        }
       
    }
}
