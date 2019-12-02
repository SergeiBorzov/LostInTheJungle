using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //public float Speed;
    public Animator animator;

    public enum TransitionParameter
    {
        Move,
        Jump,
        WalkingTurn,
    }

    public bool IsFacingForward()
    {
        if (transform.forward.x > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FaceForward(bool forward)
    {
        if (forward)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }


    void Update()
    {

    }
}
