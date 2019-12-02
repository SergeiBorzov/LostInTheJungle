using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/CheckWalkingTurn")]
public class CheckWalkingTurn : StateData
{
    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl control = characterState.GetCharacterControl(animator);

        if (control.IsFacingForward()) 
        {
            //Debug.Log("Forward");
            Debug.Log(VirtualInputManager.Instance.MoveLeft);
            if (VirtualInputManager.Instance.MoveLeft)
            {
                Debug.Log("TurnLeft");
                animator.SetBool(CharacterControl.TransitionParameter.WalkingTurn.ToString(), true);
            }
        }

        if (!control.IsFacingForward())
        {
            //Debug.Log("!Forward");
            if (VirtualInputManager.Instance.MoveRight)
            {
                Debug.Log("TurnRight");
                animator.SetBool(CharacterControl.TransitionParameter.WalkingTurn.ToString(), true);
            }
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        //animator.SetBool(CharacterControl.TransitionParameter.WalkingTurn.ToString(), false);
    }
}
