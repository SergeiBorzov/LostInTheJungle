using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        //Jump,
        //Turn,
        //Run,
        //ForceTransition,
    }

    public Animator animator;
    private CharacterController characterController;


    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;

  
    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Flip() // вызывается самой анимацией, когда переходит в состояние поворота в автомате
    {
        if (lookingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        else
        {
           transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

        lookingRight = !lookingRight;
    }

    void Update() {

        float horizontal_move = Input.GetAxis("Horizontal");
        if (horizontal_move != 0.0f) 
        {
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
        else
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        move_direction.x = horizontal_move * runSpeed;
        move_direction += Physics.gravity * gravityScale * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            move_direction.y = 0.0f;
            move_direction.y = jumpForce;
        }

        if (horizontal_move > 0.0f && !lookingRight)
        {
            //Flip();
            animator.SetBool(TransitionParameter.Turn.ToString(), true);
        }

        if (horizontal_move < 0.0f && lookingRight)
        {
            //Flip();
            animator.SetBool(TransitionParameter.Turn.ToString(), true);
        }

        if (transform.position.z != 0.0f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        characterController.Move(movementOffset + move_direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
