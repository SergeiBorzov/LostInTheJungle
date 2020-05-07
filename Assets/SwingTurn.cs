using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTurn : StateMachineBehaviour
{
    Character characterScript;
    Rigidbody m_Rigidbody;
    Hook m_Hook;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterScript = animator.GetComponentInParent<Character>();
        characterScript.isTurning = true;
        m_Hook = characterScript.GetComponentInChildren<Hook>();
       
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //characterScript.Flip();
        //m_Rigidbody.angularVelocity = Vector3.zero;
        //m_Rigidbody.Sleep();
        //m_Hook.flip = true;
        characterScript.isTurning = false;
        animator.SetBool(Character.TransitionParameter.Turn.ToString(), false);

        m_Hook.flip = true;
        // m_Rigidbody.WakeUp();

        //Vector3 v = m_Rigidbody.velocity;
        //m_Rigidbody.isKinematic = true;
        //RigidbodyConstraints c = m_Rigidbody.constraints;
        //m_Rigidbody.constraints = RigidbodyConstraints.None;
        //characterScript.Flip();

        //m_Rigidbody.isKinematic = false;
        //m_Rigidbody.constraints = c;
        //m_Rigidbody.velocity = v;
    }
}
