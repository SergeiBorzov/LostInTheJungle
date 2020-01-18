using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpearState: ICharacterState
{


    private CharacterController characterController;
    private Animator animator;
    private BoxCollider targetTrigger;
    private CharacterIK characterIKscript;
    private SpearScript spearLogic;

    private List<Transform> possibleTargetList = null;
    private List<Transform> targetList = null;
    private int targetIndex = 0;

    private Vector3 movementOffset = new Vector3();

    public void OnStateEnter(Character character)
    {
        // Script
        characterController = character.GetComponent<CharacterController>();
        animator = character.GetComponent<Animator>();
        characterIKscript = character.GetComponent<CharacterIK>();
        targetTrigger = character.GetComponent<BoxCollider>();
        spearLogic = character.spearLogic;
        spearLogic.SetCollisionStatus(false);

        targetTrigger.enabled = true;
        possibleTargetList = new List<Transform>();
        targetList = new List<Transform>();
        targetIndex = 0;



        // Animation
        animator.SetBool(Character.TransitionParameter.PickSpearThrow.ToString(), true);
    }

    private void ReleaseTargets(Character character)
    {
        if (character.Target != null && !character.ThrowingSpear)
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

    private void ChooseTarget(Character character)
    {
        character.Target = targetList[targetIndex];
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


            if (Vector3.Dot(rayDirection, character.eye.forward) < 0.0f)
            {
                animator.SetBool(Character.TransitionParameter.SpearTurn.ToString(), true);
            }

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
            //character.TargetFixed = true;

            spearLogic.SetSpearActive(true);
            character.ThrowingSpear = true;
            animator.SetBool(Character.TransitionParameter.SpearThrow.ToString(), true);
            animator.SetBool(Character.TransitionParameter.HaveSpear.ToString(), false);
            character.SetState(Character.freeMoveState);
        }

    }

    public void Update(Character character)
    {

        float horizontal_move = Input.GetAxis("Horizontal");
        bool Moving = Mathf.Abs(horizontal_move) > 0.01f;

        if (Moving)
        {
            if (Mathf.Sign(horizontal_move) == 1 && character.LookingRight || Mathf.Sign(horizontal_move) == -1 && !character.LookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Move.ToString(), true);
                animator.SetBool(Character.TransitionParameter.SpearWalkForward.ToString(), true);
                animator.SetBool(Character.TransitionParameter.SpearWalkBackwards.ToString(), false);
            }
            else if (Mathf.Sign(horizontal_move) == -1 && character.LookingRight || Mathf.Sign(horizontal_move) == 1 && !character.LookingRight)
            {
                animator.SetBool(Character.TransitionParameter.Move.ToString(), true);
                animator.SetBool(Character.TransitionParameter.SpearWalkBackwards.ToString(), true);
                animator.SetBool(Character.TransitionParameter.SpearWalkForward.ToString(), false);
            }
        }
        else
        {
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
            animator.SetBool(Character.TransitionParameter.SpearWalkBackwards.ToString(), false);
            animator.SetBool(Character.TransitionParameter.SpearWalkForward.ToString(), false);
        }

        character.moveDirection.x = horizontal_move * character.walkSpeed;


        if (Input.GetKeyDown(KeyCode.F))
        {
            // Animation
            animator.SetBool(Character.TransitionParameter.PickSpearThrow.ToString(), false);
            characterIKscript.ReleaseTarget();

            character.SetState(Character.freeMoveState);
            return;
        }

        if (character.Aiming)
        {
            FindTargets(character);
            //Debug.Log("TargetList" + targetList.Count);
            ChangeTarget(character);
            CheckTarget(character);
            ThrowSpear(character);
        }
       


        if (Mathf.Abs(character.transform.position.z) > 0.01f)
        {
            movementOffset.z = (0.0f - character.transform.position.z) * 0.1f;
        }

        ///------------------Don't move while picking spear------------------------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickSpear") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTurn"))
        {
            character.moveDirection.x = 0.0f;
        }
        ///---------------------------------------------------------------------

        characterController.Move(movementOffset + character.moveDirection * Time.deltaTime);
    }

    public void OnStateExit(Character character)
    {
        // Script
        ReleaseTargets(character);
        targetTrigger.enabled = false;
        character.Aiming = false;
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
