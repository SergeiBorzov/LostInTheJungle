using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "LostInTheJungle/AbilityData/MoveForward")]

public class MoveForward : StateData
{
    public float Speed;

    public override void UpdateAbility(CharacterState characterState, Animator animator)
    {
        CharacterControl c = characterState.GetCharacterControl(animator);

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
            c.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            c.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            //animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            c.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            c.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            //animator.SetBool(CharacterControl.TransitionParameter.Move.ToString(), true);
        }
    }
}
/*
    public class MoveForward : StateData
    {
        public float Speed;
        public override void UpdateAbility(CharacterState characterState, Animator animator)
        {
            CharacterControl c = characterState.GetCharacterControl(animator);

            if (VirtualInputManager.Instance.MoveRight && VirtualInputManager.Instance.MoveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (!VirtualInputManager.Instance.MoveRight && !VirtualInputManager.Instance.MoveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (VirtualInputManager.Instance.MoveRight)
            {
                c.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                c.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (VirtualInputManager.Instance.MoveLeft)
            {
                c.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                c.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }
}
*/
