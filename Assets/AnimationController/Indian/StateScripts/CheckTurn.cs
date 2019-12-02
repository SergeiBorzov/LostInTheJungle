using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/CheckTurn")]
public class CheckTurn : StateData
{
    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl control = characterState.GetCharacterControl(animator);

        if (control.IsFacingForward()) 
        {
            if (VirtualInputManager.Instance.MoveLeft)
            {
                animator.SetBool(CharacterControl.TransitionParameter.Turn.ToString(), true);
            }
        }

        if (!control.IsFacingForward())
        {
            if (VirtualInputManager.Instance.MoveRight)
            {
                animator.SetBool(CharacterControl.TransitionParameter.Turn.ToString(), true);
            }
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        //animator.SetBool(CharacterControl.TransitionParameter.WalkingTurn.ToString(), false);
    }
}
