using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : CharacterStateBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
            GetCharacterControl(animator).transform.Translate(Vector3.forward * GetCharacterControl(animator).Speed * Time.deltaTime);
            GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            //animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            GetCharacterControl(animator).transform.Translate(Vector3.forward * GetCharacterControl(animator).Speed * Time.deltaTime);
            GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            //animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}