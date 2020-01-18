using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class FreeMoveState: ICharacterState
{
    private Transform transform;
    private CharacterController characterController;
    private Animator animator;
    private Spear spearScript;
    private SpearScript spearLogic;
    private Vector3 movementOffset = new Vector3();
    public void OnStateEnter(Character character)
    {
        transform = character.transform;
        characterController = character.GetComponent<CharacterController>();
        animator = character.GetComponent<Animator>();
        spearScript = character.GetComponent<Spear>();
        spearLogic = character.spearLogic;


        animator.SetBool(Character.TransitionParameter.HaveSpear.ToString(), true);
    }
    public void Update(Character character)
    {
        if (Input.GetKeyDown(KeyCode.F) && !character.ThrowingSpear)
        {
            character.SetState(Character.throwSpearState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (character.TargetFixed)
            {
                spearLogic.TriggerExit();
                spearLogic.SetSpearActive(false);
                Debug.Log("Dissolve called!");
                character.Target.GetComponent<Target>().StartDissolve(character.portObjectHere.position);
                spearScript.RemoveSpear();


                animator.SetBool(Character.TransitionParameter.HaveSpear.ToString(), true);
                character.Target = null;
                character.TargetFixed = false;
            }
        }

        float horizontal_move = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal_move) > 0.01f)
        {
            animator.SetBool(Character.TransitionParameter.Move.ToString(), true);
        }
        else
        {
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
        }

        character.moveDirection.x = horizontal_move * character.runSpeed;
        ///---------------------------------------------------------------------

        ///-----------------------Check grounded--------------------------------
        if (characterController.isGrounded)
        {

            animator.SetBool(Character.TransitionParameter.isGrounded.ToString(), true);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal"))
            {
                character.moveDirection.y = -9.8f;
            }
        }
        else
        {
            animator.SetBool(Character.TransitionParameter.isGrounded.ToString(), false);
            character.moveDirection += Physics.gravity * character.gravityScale * Time.deltaTime;
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check jump----------------------------------
        if (Input.GetButtonDown("Jump") && characterController.isGrounded &&
             //!animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding") &&
             !animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump"))
        {
            animator.SetBool(Character.TransitionParameter.Jump.ToString(), true);
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
            animator.SetBool(Character.TransitionParameter.Turn.ToString(), false);
            if (Mathf.Abs(horizontal_move) > 0.01f)
            {
                character.moveDirection.y = character.jumpForce;
            }

        }
        ///---------------------------------------------------------------------

        ///--------------No new jumps and move backwards in jump---------------------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding"))
        {
            if (character.LookingRight && horizontal_move < 0.0f)
            {
                character.moveDirection.x = -(character.moveDirection.x + character.slowingDown);
            }

            if (!character.LookingRight && horizontal_move > 0.0f)
            {
                character.moveDirection.x = -(character.moveDirection.x - character.slowingDown);
            }

        }
        ///---------------------------------------------------------------------

        ///--------------------------No jump in turning-------------------------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunningTurn") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowSpear"))
        {
            character.moveDirection.x = 0.0f;
            character.moveDirection.y = 0.0f;
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check turn----------------------------------
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding"))
        {
            if (horizontal_move > 0.0f && !character.LookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
            }

            if (horizontal_move < 0.0f && character.LookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
            }
        }
        ///---------------------------------------------------------------------

        if (Mathf.Abs(transform.position.z) > 0.01f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        ///---------Don't move in standing jump or while removing spear---------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RemoveSpear") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowSpear"))
        {
            character.moveDirection.x = 0.0f;
        }
        ///---------------------------------------------------------------------
        
        characterController.Move(movementOffset + character.moveDirection * Time.deltaTime);
    }

    public void OnStateExit(Character character)
    {
        return;
    }

    public void OnTriggerEnter(Character character, Collider other)
    {
        return;
    }

    public void OnTriggerExit(Character character, Collider other)
    {
        return;
    }

}