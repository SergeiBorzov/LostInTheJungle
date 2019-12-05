using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationState : StateMachineBehaviour
{
    private Character characterControl;
    public Character GetCharacterControl(Animator animator)
    {
        return characterControl;
    }
}