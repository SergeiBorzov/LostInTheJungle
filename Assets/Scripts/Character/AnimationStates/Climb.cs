using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : StateMachineBehaviour
{
    Character characterControl;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterControl = animator.GetComponentInParent<Character>();
        animator.SetBool(Character.TransitionParameter.Jump.ToString(), false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //characterControl.enabled = false;

        //characterControl.transform.position += new Vector3(1f, 3f, 0.0f);
        //characterControl.enabled = true;
        characterControl.characterController.enabled = false;
        characterControl.transform.position = characterControl.grabbedLedge.transform.position + characterControl.grabbedLedge.endPoint;
        characterControl.characterController.enabled = true;
        //characterControl.isGrabbingLedge = false;
        //animator.SetBool(Character.TransitionParameter.isGrabbingLedge.ToString(), false);
        characterControl.transform.parent = null;
      


        //characterControl.animator.transform.position = characterControl.grabbedLedge.transform.position + characterControl.grabbedLedge.endPoint;
        //characterControl.isGrabbingLedge = false;
        //characterControl.transform.parent = null;
        //animator.SetBool(Character.TransitionParameter.Climb.ToString(), false);

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
