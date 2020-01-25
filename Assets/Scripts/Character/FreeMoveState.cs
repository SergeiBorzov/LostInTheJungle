using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoveState : ICharacterState
{
    private GameObject character;
    private Character characterScript;
    private CharacterController characterController;
    private Animator animator;

    private Vector3 movementOffset = Vector3.zero;
    #region MoveFields
    [SerializeField] public float runSpeed = 7.0f;
    [SerializeField] public float walkSpeed = 3.0f;
    [SerializeField] public float pushForce = 5.0f;
    [SerializeField] public float slowingDown = 2.0f;
    #endregion

    private bool isTurning = false;

    private Vector3 velocity = Vector3.zero;
   
    public void OnStateEnter(GameObject ch)
    {
        character = ch;
        characterScript = character.GetComponent<Character>();
        characterController = character.GetComponent<CharacterController>();
        animator = character.GetComponent<Animator>();
    }


    private void Move(Vector3 velocity)
    {
        if (characterScript.moveOn)
        {
            if (Mathf.Abs(character.transform.position.z) > 0.01f)
            {
                movementOffset.z = (0.0f - character.transform.position.z) * 2.0f;
            }

            if (!(characterScript.groundAngle > characterScript.maxGroundAngle))
            {
                characterController.Move((movementOffset + velocity) * Time.deltaTime);
            }
        }
    }

    private void SetAnimatorMoveState(float horizontalMove)
    {
        if (Mathf.Abs(horizontalMove) > 0.01f && !characterScript.isJumping && !characterScript.isDroping)
        {
            animator.SetBool(Character.TransitionParameter.Move.ToString(), true);
        }
        else
        {
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
        }
    }

    private void SetAnimatorTurnState(float horizontalMove)
    {
        ///-------------------------Check turn----------------------------------
        if (!characterScript.isLanding)
        {
            if (horizontalMove > 0.0f && !characterScript.lookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
            }


            if (horizontalMove < 0.0f && characterScript.lookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
            }
        }
    }

    private void SetAnimatorClimbState()
    {
        if (Input.GetKeyDown(KeyCode.W) && characterScript.isGrabbingLedge)
        {
            animator.SetBool(Character.TransitionParameter.Climb.ToString(), true);
        }
        if (Input.GetKeyDown(KeyCode.S) && characterScript.isGrabbingLedge && !characterScript.isClimbing)
        {
            characterScript.isGrabbingLedge = false;
            animator.SetBool(Character.TransitionParameter.isGrabbingLedge.ToString(), false);
        }
    }

    private void SetAnimatorHangingState()
    {
        if (characterScript.isGrabbingLedge)
        {
            velocity.x = 0.0f;
            characterScript.verticalVelocity = Vector3.zero;
            characterScript.gravityOn = false;
        }
    }


    

    private void SetAnimatorJumpState(float horizontalMove)
    {
        if (Input.GetKeyDown(KeyCode.W) && characterScript.isGrounded
            && !characterScript.isTurning && !characterScript.isJumping && !characterScript.isDroping)
        {
            animator.SetBool(Character.TransitionParameter.Jump.ToString(), true);
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
            animator.SetBool(Character.TransitionParameter.Turn.ToString(), false);


            if (Mathf.Abs(horizontalMove) > 0.01f && !characterScript.isIdle)
            {
                animator.SetBool(Character.TransitionParameter.RunningJump.ToString(), true);
                characterScript.verticalVelocity.y = characterScript.jumpForce;
            }

        }

    }

    private void SetGravity()
    {
        if (characterScript.isGrounded)
        {
            if (!characterScript.isJumping)
            {
                characterScript.verticalVelocity.y = -5.0f;
            }
            characterScript.gravityOn = false;
        }
        else
        {
            characterScript.gravityOn = true;
        }
    }

    public void Update()
    {
       
        float horizontalMove = Input.GetAxis("Horizontal");

        SetAnimatorClimbState();
        SetAnimatorMoveState(horizontalMove);
        velocity = characterScript.forward * Mathf.Abs(horizontalMove) * runSpeed;

        SetGravity();
        SetAnimatorJumpState(horizontalMove);
        SetAnimatorHangingState();
        SetAnimatorTurnState(horizontalMove);
        Move(velocity);
    }

    public void OnStateExit()
    {
        return;
    }

    public void OnTriggerEnter(Collider other)
    {
        return;
    }

    public void OnTriggerExit(Collider other)
    {
        return;
    }


}
