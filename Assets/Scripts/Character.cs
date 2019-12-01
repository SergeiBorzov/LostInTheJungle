using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    private Animator animator;
    private CharacterController characterController;
    //private Animation animation;


    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;
    private bool isInAir = false;
    private bool characterTurning = false;

    /* Animator parameters indices*/
    int animatorSpeedIndex;
    int animatorJumpIndex;
    int animatorWalkingJumpIndex;
    int animatorRunningJumpIndex;
    int animatorRunningTurn180;

    /* Animator state indices */
    int animatorStateIdleIndex;
    int animatorStateWalkIndex;
    int animatorStateRunIndex;
    int animatorStateMoveIndex;


    void ChangeIsInAir()
    {
        isInAir = !isInAir;
    }

    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        animatorSpeedIndex = Animator.StringToHash("Speed");
        animatorJumpIndex = Animator.StringToHash("Jump");
        animatorWalkingJumpIndex = Animator.StringToHash("WalkingJump");
        animatorRunningJumpIndex = Animator.StringToHash("RunningJump");
        animatorRunningTurn180 = Animator.StringToHash("RunningTurn180");

        animatorStateIdleIndex = Animator.StringToHash("Base Layer.Idle");
        // animatorStateWalkIndex = Animator.StringToHash("Base Layer.Walk");
        // animatorStateRunIndex = Animator.StringToHash("Base Layer.Run");
        animatorStateMoveIndex = Animator.StringToHash("Base Layer.Move");

    }

    private void Flip()
    {
      /*  AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == animatorStateRunIndex)
        {
            animator.SetTrigger(animatorRunningTurn180);
        }*/
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.z *= -1;
        transform.localScale = scale;

    }

    /*
    private void Turning()
    {
        Vector3 scale = transform.localScale;
        scale.z *= -1;
        transform.localScale = scale;
    }
    */

    void Update() {
        /* Get Horizontal speed -1..1 */
        float horizontal_move = 0.0f;
        if (!isInAir)
            horizontal_move = Input.GetAxis("Horizontal");

        /* Deal with animation parameters */
       

        /* Face character the right way */
        if (horizontal_move > 0.1f && !lookingRight && !isInAir)
        {
            Flip();
        }
        else if (horizontal_move < -0.1f && lookingRight && !isInAir)
        {
            Flip();
        }

        if (!characterTurning)
            animator.SetFloat(animatorSpeedIndex, Mathf.Abs(horizontal_move));

        /* Handle Jump */
        if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.fullPathHash == animatorStateIdleIndex)
            {
                animator.SetTrigger(animatorJumpIndex);
            }

            if (stateInfo.fullPathHash == animatorStateMoveIndex)
            {
                animator.SetTrigger(animatorRunningJumpIndex);
            }
            /*
            if (stateInfo.fullPathHash == animatorStateIdleIndex)
            {
                animator.SetTrigger(animatorJumpIndex);
            }

            if (stateInfo.fullPathHash == animatorStateWalkIndex)
            {
                animator.SetTrigger(animatorWalkingJumpIndex);
            }

            if (stateInfo.fullPathHash == animatorStateRunIndex)
            {
                animator.SetTrigger(animatorRunningJumpIndex);
            }
            */
        }


        /* Construct velocity vector */
        move_direction = new Vector3(horizontal_move*runSpeed, move_direction.y, move_direction.z);
        move_direction.y += (Physics.gravity.y * gravityScale * Time.deltaTime);

          
        if (transform.position.z != 0.0f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
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