using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;

    [SerializeField]
    private GateButton button1;

    [SerializeField]
    private GateButton button2;

    [SerializeField]
    private GateButton button3;

    int current = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }


    public bool Check(int order)
    {
        if (order == 1 && current == 0)
        {
            current = 1;
            return true;
        }
        else if (order == 2 && current == 1)
        {
            current = 2;
            return true;
        }
        else if (order == 3 && current == 2)
        {
            OpenGates();
            return true;
        }
        else
        {
            Reset();
            return false;
        }
    }

    private void Reset()
    {
        current = 0;
        button1.Reset();
        button2.Reset();
        button3.Reset();
    }

    public void OpenGates()
    {
        button1.Die();
        button2.Die();
        button3.Die();
        animator.enabled = true;
        this.GetComponents<AudioSource>()[0].Play();
    }
}
