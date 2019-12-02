using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/Idle")]

public class Idle : StateData
{
    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (VirtualInputManager.Instance.Jump)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Jump.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveRight && VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), false);
            return;
        }
        if (!VirtualInputManager.Instance.MoveRight && !VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), false);
            return;
        }

        if (VirtualInputManager.Instance.MoveRight)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);

        }

        if (VirtualInputManager.Instance.Jump)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Jump.ToString(), true);
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}