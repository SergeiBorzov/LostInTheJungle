using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpearState: ICharacterState
{


    private CharacterController characterController;
    private Animator animator;
    private BoxCollider targetTrigger;
    private CharacterIK characterIKscript;

    private List<Transform> possibleTargetList = null;
    private List<Transform> targetList = null;
    private int targetIndex = 0;


    public void OnStateEnter(Character character)
    {
        // Script
        characterController = character.GetComponent<CharacterController>();
        animator = character.GetComponent<Animator>();
        characterIKscript = character.GetComponent<CharacterIK>();
        targetTrigger = character.GetComponent<BoxCollider>();
        targetTrigger.enabled = true;
        possibleTargetList = new List<Transform>();
        targetList = new List<Transform>();
        targetIndex = 0;

        // Animation
        animator.SetBool(Character.TransitionParameter.PickSpearThrow.ToString(), true);
    }

    private void ReleaseTargets(Character character)
    {
        if (character.Target != null && !character.TargetFixed)
        {
            character.Target.gameObject.GetComponent<Target>().SetDefaultOutline();
            character.Target = null;
        }
        possibleTargetList.Clear();
        targetList.Clear();
        targetIndex = 0;
    }

    private void FindTargets(Character character)
    {
        if (possibleTargetList.Count != 0)
        {
            targetList.Clear();
            foreach (Transform possibleTarget in possibleTargetList)
            {
                Vector3 rayDirection = possibleTarget.position - character.eye.position;
                rayDirection.Normalize();

                if (Vector3.Dot(rayDirection, character.eye.forward) > 0.0f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(character.eye.position, rayDirection, out hit, Mathf.Infinity))
                    {
                        // Don't delete useful for debug
                        Debug.DrawRay(character.eye.position, rayDirection * 10, Color.green);

                        if (hit.transform.gameObject.name == possibleTarget.name)
                        {
                            targetList.Add(possibleTarget);
                            //target = possibleTarget;
                            //target.gameObject.GetComponent<Target>().ChangeMaterial();
                        }
                    }
                }
            }
        }
    }

    private void ChooseTarget(Character character)
    {
        character.Target = targetList[targetIndex];
        Debug.Log("I'm here!");
        character.Target.gameObject.GetComponent<Target>().ChangeOutline();
        characterIKscript.SetTarget(character.Target);
    }

    private void ChangeTarget(Character character)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (targetList.Count != 0 && targetList.Count != 1)
            {
                targetIndex = ((targetIndex + 1) % targetList.Count + targetList.Count) % targetList.Count;
                // ??????????????
                if (character.Target != null)
                {
                    character.Target.GetComponent<Target>().ChangeOutline();
                }
                ChooseTarget(character);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (targetList.Count != 0 && targetList.Count != 1)
            {
                targetIndex = ((targetIndex - 1) % targetList.Count + targetList.Count) % targetList.Count;
                if (character.Target != null)
                {
                    character.Target.GetComponent<Target>().ChangeOutline();
                }
                ChooseTarget(character);
            }
        }
    }

    private void CheckTarget(Character character)
    {
        if (character.Target != null)
        {
            Vector3 rayDirection = character.Target.position - character.eye.position;

            RaycastHit hit;
            if (Physics.Raycast(character.eye.position, rayDirection, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.CompareTag("Target"))
                {
                    return;
                }

                if (!character.TargetFixed)
                {
                    Debug.Log("MSG 1");
                    targetList.Remove(character.Target);
                    character.Target.gameObject.GetComponent<Target>().SetDefaultOutline();
                    character.Target = null;
                    targetIndex = 0;
                }

            }
            else
            {
                if (!character.TargetFixed)
                {
                    Debug.Log("MSG 2");
                    targetList.Remove(character.Target);
                    character.Target.gameObject.GetComponent<Target>().SetDefaultOutline();
                    character.Target = null;
                    targetIndex = 0;
                }
            }
        }

        if (targetList.Count != 0)
        {
            ChooseTarget(character);
        }
    }

    private void ThrowSpear(Character character)
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Script
            character.TargetFixed = true;
            animator.SetBool(Character.TransitionParameter.SpearThrow.ToString(), true);
            animator.SetBool(Character.TransitionParameter.HaveSpear.ToString(), false);
            character.SetState(Character.freeMoveState);
        }

    }

    public void Update(Character character)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Animation
            animator.SetBool(Character.TransitionParameter.PickSpearThrow.ToString(), false);
            characterIKscript.ReleaseTarget();

            character.SetState(Character.freeMoveState);
            return;
        }

        FindTargets(character);
        Debug.Log("TargetList" + targetList.Count);
        ChangeTarget(character);
        CheckTarget(character);
        ThrowSpear(character);
    }
    
    public void OnStateExit(Character character)
    {
        // Script
        ReleaseTargets(character);
        targetTrigger.enabled = false;
    }

    public void OnTriggerEnter(Character character, Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            possibleTargetList.Add(other.gameObject.transform);
        }
    }


    public void OnTriggerExit(Character character, Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            possibleTargetList.Remove(other.gameObject.transform);
            targetList.Remove(other.gameObject.transform);
            if (character.Target == other.gameObject.transform)
            {
                other.gameObject.GetComponent<Target>().SetDefaultOutline();
                character.Target = null;
            }
            targetIndex = 0;
        }
    }


}
