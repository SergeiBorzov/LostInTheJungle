using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private float pushForce = 5.0f;

    [SerializeField] private float slowingDownСoeff = 5.0f;

    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        Jump,
        isGrounded,
    }

    public enum MovementState
    {
        FreeMove,
        Rope,
        JumpOffRope
    }

    private Animator animator;
    private CharacterController characterController;
    private Rigidbody rigidBody;
    private Rope ropeScript;

    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;

    MovementState currentState = MovementState.FreeMove;

    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //cableComponent = GetComponent<CableComponent>();
    }

    public void Flip() // вызывается самой анимацией, когда переходит в состояние поворота в автомате
    {
        if (lookingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        else
        {
           transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

        lookingRight = !lookingRight;
    }

    public void StandingJump() // вызывается внутри самой анимации (не в автомате)
    {
        move_direction.y = jumpForce;
    }

    
    private void Movement()
    {
        ////------------------------Check move----------------------------------

        float horizontal_move = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal_move) > 0.01f)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
        else
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        
        move_direction.x = horizontal_move * runSpeed;
       
       


        ///---------------------------------------------------------------------

        ///-----------------------Check grounded--------------------------------
        if (characterController.isGrounded)
        {
            
            animator.SetBool(TransitionParameter.isGrounded.ToString(), true);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal"))
            {
                move_direction.y = -9.8f;
            }
        }
        else
        {
            animator.SetBool(TransitionParameter.isGrounded.ToString(), false);
            move_direction += Physics.gravity * gravityScale * Time.deltaTime;
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check jump----------------------------------
        if ( Input.GetButtonDown("Jump") && characterController.isGrounded && 
             !animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding") &&
             !animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump") )
        {
            animator.SetBool(Character.TransitionParameter.Jump.ToString(), true);
            animator.SetBool(Character.TransitionParameter.Move.ToString(), false);
            animator.SetBool(Character.TransitionParameter.Turn.ToString(), false);
            if (Mathf.Abs(horizontal_move) > 0.01f)
            {
                move_direction.y = jumpForce;
            }

        }
        ///---------------------------------------------------------------------
         
        ///--------------No new jumps and move backwards in jump---------------------
        if ( animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump") ||
             animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding"))
        {
            if (lookingRight && horizontal_move < 0.0f)
            {
                move_direction.x = -(move_direction.x + slowingDownСoeff);
            }

            if (!lookingRight && horizontal_move > 0.0f)
            {
                move_direction.x = -(move_direction.x - slowingDownСoeff);
            }

        }
        ///---------------------------------------------------------------------

        ///--------------------------No jump in turning-------------------------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunningTurn"))
        {
            move_direction.x = 0.0f;
            move_direction.y = 0.0f;
        }
        ///---------------------------------------------------------------------

        ///-------------------------Check turn----------------------------------
        if ( !animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump") && 
             !animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJumpLanding") )
        {
            if (horizontal_move > 0.0f && !lookingRight)
            {
                animator.SetBool(TransitionParameter.Turn.ToString(), true);
            }

            if (horizontal_move < 0.0f && lookingRight)
            {
                animator.SetBool(TransitionParameter.Turn.ToString(), true);
            }
        }
        ///---------------------------------------------------------------------

        if (Mathf.Abs(transform.position.z) > 0.01f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        ///------------------Don't move in standing jump------------------------
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalPrep") ||
             animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormalLanding") ||
             animator.GetCurrentAnimatorStateInfo(0).IsName("JumpNormal"))
        {
            move_direction.x = 0.0f;
        }
        ///---------------------------------------------------------------------

        characterController.Move(movementOffset + move_direction * Time.deltaTime);
    }

    /*Vector3 RopeClimbing(bool up)
    {
        CapsuleCollider currentSegmentCollider = ropeScript.GiveCurrentSegment(transform.position);

        if (currentSegmentCollider == null)
        {
            Debug.Log("Strange bug");
            return new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            transform.SetParent(currentSegmentCollider.transform);
            Vector3 offset = currentSegmentCollider.transform.up * Time.deltaTime;
            if (!up)
            {
                offset *= -1;
            }
            return transform.position + offset;

        }
    }*/

    private void MovementOnRope()
    {
        float forceCoefficient;
        float swingPower = 0.5f;

        /* Swinging */
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {

            forceCoefficient = Mathf.Clamp(Vector3.Dot(transform.up, Vector3.right), 0, 1);
            rigidBody.AddForce(swingPower*forceCoefficient*Vector3.right, ForceMode.Impulse);

        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            forceCoefficient = Mathf.Clamp(Vector3.Dot(transform.up, Vector3.left), 0, 1);
            rigidBody.AddForce(swingPower*forceCoefficient * Vector3.left, ForceMode.Impulse);
        }


        /*if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            //transform.position = RopeClimbing(true);
        }
        else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            //transform.position = RopeClimbing(false);
        }*/

        if (Input.GetButtonDown("Jump"))
        {
            characterController.enabled = true;
            transform.parent = null;

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(-transform.rotation.x, 90, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(-transform.rotation.x, -90, 0);
            }

            ropeScript.SetDefaultEndPoint(transform);
            currentState = MovementState.JumpOffRope;
            move_direction.y = jumpForce;
            move_direction.x = horizontal * runSpeed;

        }
    }

    private void JumpOffRope()
    {
       
        move_direction += Physics.gravity * gravityScale * Time.deltaTime;
        characterController.Move(movementOffset + move_direction * Time.deltaTime);
        if (characterController.isGrounded)
        {
            currentState = MovementState.FreeMove;
        }
    }

    void Update() {
        switch (currentState)
        {
            case MovementState.FreeMove:
            {
                Movement();
                break;
            }

            case MovementState.Rope:
            {
                MovementOnRope();
                break;
            }

            case MovementState.JumpOffRope:
            {
                JumpOffRope();
                break;
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rope"))
        {
            if (currentState == MovementState.FreeMove) {
                characterController.enabled = false;
                transform.SetParent(other.gameObject.transform);
                

                ropeScript = other.gameObject.GetComponent<Rope>();
                ropeScript.CreateEndPoint(transform);
                rigidBody = gameObject.GetComponent<Rigidbody>();



                currentState = MovementState.Rope;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rope"))
        {
            currentState = MovementState.FreeMove;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Rigidbody body = hit.collider.attachedRigidbody;
        
        if (body == null || body.isKinematic)
            return;

       
        // We want to push objects below us
        if (hit.moveDirection.y < -0.3f)
        {
            Vector3 pushDir = new Vector3(0, hit.moveDirection.y, 0);
            body.velocity = pushDir * pushForce;
        }
        
        // We want to push objects in front of us
        if (Mathf.Abs(hit.moveDirection.x) > 0.3f)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
            body.velocity = pushDir * pushForce;
        }

    }
}
