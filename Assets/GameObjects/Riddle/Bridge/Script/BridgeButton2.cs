using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton2 : MonoBehaviour
{
    private Animator animator;
    private Material mat;
    private float lightUpTime = 2.0f;
    private float timer = 1.0f;

    private bool active;
    [SerializeField]
    private bool alive;

    [SerializeField]
    Bridge bridge2;

    private int m_Activators = 0;

    public bool IsActive()
    {
        return active;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        active = false;
        alive = true;
        mat = GetComponent<Renderer>().material;
        timer = lightUpTime;
    }

    public void Die()
    {
        alive = false;
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

            mat.SetColor("_EmissionColor", Color.yellow * ((lightUpTime - timer) / lightUpTime));
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

    private void OnTriggerStay(Collider other)
    {
        if (alive)
        {
            var characterScript = other.gameObject.GetComponent<Character>();
            var box = other.gameObject.GetComponent<Box>();
            if (characterScript != null || box != null)
            {
                if (!active)
                {
                    active = true;
                    StopCoroutine("LightDown");
                    StartCoroutine("LightUp");
                    animator.SetBool("Active", active);
                    bridge2.Action();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (alive)
        {
            var characterScript = other.gameObject.GetComponent<Character>();
            var box = other.gameObject.GetComponent<Box>();

            if (characterScript != null || box != null)
            {
                if (active)
                {
                    active = false;
                   
                    StopCoroutine("LightUp");
                    StartCoroutine("LightDown");
                

                    animator.SetBool("Active", active);
                    bridge2.Action();
                }
            }

        }

    }


}
