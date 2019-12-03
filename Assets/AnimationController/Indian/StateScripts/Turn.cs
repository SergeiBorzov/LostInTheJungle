using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/Turn")]
public class Turn : StateData
{
    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        /*
        CharacterControl control = characterState.GetCharacterControl(animator);

        if (control.IsFacingForward())
        {
            control.FaceForward(false);
        }
        else
        {
            control.FaceForward(true);
        }
        */
       //animator.SetBool(CharacterControl.TransitionParameter.Turn.ToString(), false);
    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl control = characterState.GetCharacterControl(animator);

        if (control.IsFacingForward())
        {
            control.FaceForward(false);
        }
        else
        {
            control.FaceForward(true);
        }
    }
}