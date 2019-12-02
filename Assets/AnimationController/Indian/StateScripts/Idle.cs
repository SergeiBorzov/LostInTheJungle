using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/Idle")]

public class Idle : StateData
{
    public override void UpdateAbility(CharacterState characterState, Animator animator)
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
            //GetCharacterControl(animator).transform.Translate(Vector3.forward * GetCharacterControl(animator).Speed * Time.deltaTime);
            //GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            //GetCharacterControl(animator).transform.Translate(Vector3.forward * GetCharacterControl(animator).Speed * Time.deltaTime);
            //GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);

        }

        if (VirtualInputManager.Instance.Jump)
        {
            animator.SetBool(CharacterControl.TransitionParameter.Jump.ToString(), true);
        }
    }
}