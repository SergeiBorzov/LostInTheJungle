using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/Jump")]

public class Jump : StateData
{
    public float Speed;
    private bool isJump = false;

    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
       /*
         CharacterControl control = characterState.GetCharacterControl(animator);
        Vector3 velocity = control.velocity;

        if (VirtualInputManager.Instance.MoveRight)
        {
            control.controller.Move(velocity);

        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            control.controller.Move(velocity);

        }
        */       
        //+velocity_y
    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl control = characterState.GetCharacterControl(animator);
        Vector3 velocity = control.velocity;

        //if (VirtualInputManager.Instance.MoveRight)
        //{
        //Debug.Log(isJump);
        if (!isJump)
        {
            control.controller.Move(velocity);
            isJump = true;
        }
        //}

        //if (VirtualInputManager.Instance.MoveLeft)
        //{
        //    control.controller.Move(velocity);
        //}

        /*
        CharacterControl control = characterState.GetCharacterControl(animator);

        if (VirtualInputManager.Instance.MoveRight)
        {
            control.controller.Move(Vector3.right * Speed * Time.deltaTime);

        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            control.controller.Move(Vector3.left * Speed * Time.deltaTime);

        }
        */
        //+gravitation

    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        animator.SetBool(CharacterControl.TransitionParameter.Jump.ToString(), false);
        isJump = false;
    }
}


