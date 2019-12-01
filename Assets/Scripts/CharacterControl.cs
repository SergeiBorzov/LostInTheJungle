using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float Speed;
    public Animator animator;

    public enum TransitionParameter
    {
        Move,
    }

    void Update()
    {
        if (VirtualInputManager.Instance.MoveRight && VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
            return;
        }
        if (!VirtualInputManager.Instance.MoveRight && !VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {
            this.gameObject.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            this.gameObject.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            animator.SetBool(TransitionParameter.Move.ToString(), true);

        }
    }
}