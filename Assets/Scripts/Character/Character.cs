using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private float pushForce = 5.0f;

    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        Jump,
        isGrounded,
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

    public void StandingJump()
    {
        //move_direction.y = 0.0f;
        move_direction.y = jumpForce;
        //Debug.Log("Character y velocity" + move_direction.y);
    }

    void Update() {
        // move_direction = new Vector3(0.0f, 0.0f, 0.0f);
        ////------------------------Check move----------------------------------
        float horizontal_move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal_move) > 0.01f)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
        else
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        move_direction.x = horizontal_move * runSpeed;

        ///---------------------------------------------------------------------

        ///-----------------------Check grounded--------------------------------
        if (characterController.isGrounded)
        {
            animator.SetBool(TransitionParameter.isGrounded.ToString(), true);

            if ( !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal") )
            {
                //Debug.Log("Jumping");
                move_direction.y = -9.8f;
            }
            //move_direction.y = -9.8f;
        }
        else
        {
            animator.SetBool(TransitionParameter.isGrounded.ToString(), false);
            move_direction += Physics.gravity * gravityScale * Time.deltaTime;
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check jump----------------------------------
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            animator.SetBool(Character.TransitionParameter.Jump.ToString(), true);
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
            animator.SetBool(Character.TransitionParameter.Turn.ToString(), false);
            if (Mathf.Abs(horizontal_move) > 0.01f)
            {
                //move_direction.y = 0.0f;
                move_direction.y = jumpForce;
            }
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check turn----------------------------------
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
        ///---------------------------------------------------------------------

        if (Mathf.Abs(transform.position.z) > 0.01f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        ///------------------Don't move in standing jump------------------------
        if ( animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") ||
             animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") || 
             animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal") )
        {
            //Debug.Log("Jumping");
            move_direction.x = 0.0f;
        }
        ///---------------------------------------------------------------------

        characterController.Move(movementOffset + move_direction * Time.deltaTime);

       // Debug.Log("Character y velocity" + move_direction.y);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We want to push objects below us
        if (hit.moveDirection.y < -0.3f)
        {
            Vector3 pushDir = new Vector3(0, hit.moveDirection.y, 0);
            body.velocity = pushDir * pushForce;
        }
    }

}
