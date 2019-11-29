﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    private Animator animator;
    private CharacterController characterController;


    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;
    private bool isInAir = false;

    /* Animator parameters indices*/
    int animatorSpeedIndex;
    int animatorRunningJumpIndex;
    int animatorJumpIndex;

    /* Animator state indices */
    int animatorStateRunIndex;
    int animatorStateIdleIndex;

  

 
    void ChangeIsInAir()
    {
        isInAir = !isInAir;
    }

    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        animatorSpeedIndex = Animator.StringToHash("Speed");
        animatorRunningJumpIndex = Animator.StringToHash("RunningJump");
        animatorJumpIndex = Animator.StringToHash("Jump");

        animatorStateRunIndex = Animator.StringToHash("Base Layer.Run");
        animatorStateIdleIndex = Animator.StringToHash("Base Layer.Idle");
    }

    private void Flip()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.z *= -1;
        transform.localScale = scale;
    }

    void Update() {
        /* Get Horizontal speed -1..1 */
        float horizontal_move = 0.0f;
        if (!isInAir)
            horizontal_move = Input.GetAxis("Horizontal");

        /* Deal with animation parameters */
        animator.SetFloat(animatorSpeedIndex, Mathf.Abs(horizontal_move));

        /* Face character the right way */
        if (horizontal_move > 0.1f && !lookingRight && !isInAir)
        {
            Flip();
        }
        else if (horizontal_move < -0.1f && lookingRight && !isInAir)
        {
            Flip();
        }


        /* Handle Jump */
        if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.fullPathHash == animatorStateRunIndex) {
                animator.SetTrigger(animatorRunningJumpIndex);
            }
            if (stateInfo.fullPathHash == animatorStateIdleIndex)
            {
                animator.SetTrigger(animatorJumpIndex);
            }
        }


        /* Construct velocity vector */
        move_direction = new Vector3(horizontal_move*runSpeed, move_direction.y, move_direction.z);
        move_direction.y += (Physics.gravity.y * gravityScale * Time.deltaTime);

          
        if (transform.position.z != 0)
        {
            movementOffset.z = (0 - transform.position.z) * 0.1f;
        }

        if (!isInAir)
        {
            characterController.Move(movementOffset + move_direction * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }

}