using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float gravityScale = 5.0f;
    public float Speed;
    public float jumpForce;
    public Animator animator;
    public CharacterController controller;
    public Vector3 velocity;
    

    public enum TransitionParameter
    {
        Move,
        Jump,
        Turn,
        Run,
        ForceTransition,
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
        bool facingForward = IsFacingForward();

        if (facingForward)
        {
            velocity = Vector3.right * Speed * Time.deltaTime;
        }
        else
        {
            velocity = Vector3.left * Speed * Time.deltaTime;
        }

        if (!controller.isGrounded)
        {
            velocity.y += (Physics.gravity.y * gravityScale * Time.deltaTime);
        }
        else
        {
            velocity.y = 0.0f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += jumpForce;
        }

    }
}
