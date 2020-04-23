using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoveState : ICharacterState
{
    private GameObject character;
    private Character characterScript;
    private CharacterController characterController;
    private Animator animator;

    private Ledge grabbedLedge;

    private Vector3 movementOffset = Vector3.zero;
    #region MoveFields
    private float runSpeed = 7.0f;
    //private float walkSpeed = 3.0f;
    //private float pushForce = 5.0f;
    //private float slowingDown = 2.0f;
    private float timeToFallJump = 0.75f;
    private float timeToFallNoJump = 0.1f;
    private float timeLandingNeeded = 0.3f;
    private float fightSpeed = 1.0f;
    private float timeFallingJump;
    private float timeFallingNoJump;
    private float timeFalling;

    private float timeToCombo = 0.7f;
    private float clickTime;
    #endregion

    private Vector3 velocity = Vector3.zero;
   
    public void OnStateEnter(GameObject ch)
    {
        character = ch;
        characterScript = character.GetComponent<Character>();
        characterController = character.GetComponent<CharacterController>();
        animator = character.GetComponent<Animator>();
        timeFallingJump = timeToFallJump;
        timeFallingNoJump = timeToFallNoJump;
    }


    private void Move(Vector3 velocity)
    {
        if (characterScript.moveOn)
        {
            if (Mathf.Abs(character.transform.position.z) > 0.01f)
            {
                movementOffset.z = (0.0f - character.transform.position.z) * 2.0f;
            }

            if (characterScript.isFight || characterScript.isFightEnd)
            {
                characterController.Move((movementOffset + characterScript.forward * fightSpeed) * Time.deltaTime);
            }
            else if (!(characterScript.groundAngle > characterScript.maxGroundAngle))
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
        if (!characterScript.isLanding && !characterScript.isGrabbingLedge && !characterScript.isHangJumping)
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
        if ( ((characterScript.lookingRight && Input.GetKey(KeyCode.A)) ||
               (!characterScript.lookingRight && Input.GetKey(KeyCode.D))) &&
                Input.GetKeyDown(KeyCode.Space) &&
                characterScript.isGrabbingLedge &&
                !characterScript.isClimbing)
        {
            characterScript.isHangJumping = true;
            animator.SetBool(Character.TransitionParameter.isGrabbingLedge.ToString(), false);
            animator.SetBool(Character.TransitionParameter.Jump.ToString(), true);
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && characterScript.isGrabbingLedge && !characterScript.isClimbing && !characterScript.isHangJumping)
            {
                if (characterScript.IsClimbLedge())
                {
                    animator.SetBool(Character.TransitionParameter.Climb.ToString(), true);
                }
                else if (characterScript.NextClimbExists())
                {
                    animator.SetBool(Character.TransitionParameter.HangUp.ToString(), true);
                }
            }
            else if (Input.GetKey(KeyCode.S) && characterScript.isGrabbingLedge && !characterScript.isClimbing && !characterScript.isHangJumping)
            {
                if (characterScript.IsDropLedge())
                {
                    characterScript.isGrabbingLedge = false;
                    animator.SetBool(Character.TransitionParameter.isGrabbingLedge.ToString(), false);
                }
                else
                {
                    animator.SetBool(Character.TransitionParameter.HangDown.ToString(), true);
                }
            }
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
        if (Input.GetKeyDown(KeyCode.Space) && characterScript.isGrounded
            && !characterScript.isTurning && !characterScript.isJumping && !characterScript.isDroping
            && !characterScript.isFight && !characterScript.isFightEnd)
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

    private void SetFallingState()
    {
        if (!characterScript.isGrounded && !characterScript.isGrabbingLedge)
        {
            if (characterScript.isIdle || characterScript.isRunning)
            {
                timeFallingNoJump -= Time.deltaTime;
                if (timeFallingNoJump < 0.0f)
                {
                    characterScript.isFalling = true;
                    animator.SetBool(Character.TransitionParameter.Falling.ToString(), true);
                }
                if (timeFallingNoJump < -timeLandingNeeded)
                {
                    animator.SetBool(Character.TransitionParameter.LandingNeeded.ToString(), true);
                }
            }
            else
            {
                timeFallingJump -= Time.deltaTime;
                if (timeFallingJump < 0.0f)
                {
                    characterScript.isFalling = true;
                    animator.SetBool(Character.TransitionParameter.Falling.ToString(), true);
                }
                if (timeFallingJump < -timeLandingNeeded)
                {
                    animator.SetBool(Character.TransitionParameter.LandingNeeded.ToString(), true);
                }
            }
        }
        else
        {
            characterScript.isFalling = false;
            timeFallingJump = timeToFallJump;
            timeFallingNoJump = timeToFallNoJump;
            animator.SetBool(Character.TransitionParameter.Falling.ToString(), false);
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

    private void SetAnimatorFight(float horizontalMove)
    {
        if (Time.time - clickTime > timeToCombo)
        {
            characterScript.clicks = 0;
        }

        if (Input.GetMouseButtonDown(0) && (characterScript.isIdle || characterScript.isRunning || characterScript.isFight))
        {
            clickTime = Time.time;
            characterScript.clicks++;
            if (characterScript.clicks == 1)
            {
                animator.SetBool(Character.TransitionParameter.Fight.ToString(), true);
            }
           // Debug.Log(1);
            characterScript.clicks = Mathf.Clamp(characterScript.clicks, 0, 3);
        }
    }

    public void Update()
    {
        Debug.Log("Combo " + characterScript.clicks);
        SetFallingState();
        float horizontalMove = Input.GetAxis("Horizontal");

        SetAnimatorClimbState();
        SetAnimatorMoveState(horizontalMove);
        velocity = characterScript.forward * Mathf.Abs(horizontalMove) * runSpeed;
        SetGravity();
        SetAnimatorJumpState(horizontalMove);
        SetAnimatorHangingState();
        SetAnimatorTurnState(horizontalMove);
        SetAnimatorFight(horizontalMove);
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
