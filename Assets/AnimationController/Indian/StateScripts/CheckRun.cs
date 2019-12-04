using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/CheckRun")]

public class CheckRun : StateData
{
    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (VirtualInputManager.Instance.Run)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Run.ToString(), true);
        }

        if (!VirtualInputManager.Instance.Run)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Run.ToString(), false);
        }

    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}




