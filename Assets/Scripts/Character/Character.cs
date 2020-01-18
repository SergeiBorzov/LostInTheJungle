using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICharacterState
{
    void OnStateEnter(Character character);
    void Update(Character character);

    void OnStateExit(Character character);

    void OnTriggerEnter(Character character, Collider other);
    void OnTriggerExit(Character character, Collider other);
}

public class Character : MonoBehaviour
{
    #region EditorVariables
    [SerializeField] public float runSpeed = 7.0f;
    [SerializeField] public float walkSpeed = 3.0f;
    [SerializeField] public float gravityScale = 3.0f;
    [SerializeField] public float jumpForce = 10.0f;
    [SerializeField] public float pushForce = 5.0f;
    [SerializeField] public float slowingDown = 2.0f;
    [SerializeField] public Transform portObjectHere;
    [SerializeField] public Transform eye;
    [SerializeField] public SpearScript spearLogic;
    #endregion

    #region Fields
    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        Jump,
        isGrounded,
        PickSpearThrow,
        SpearThrow,
        HaveSpear,
        SpearWalkForward,
        SpearWalkBackwards,
        SpearTurn,
        isGrabbingLedge
    }

    [HideInInspector]
    public ICharacterState currentState;
    public static FreeMoveState freeMoveState = new FreeMoveState();
    public static ThrowSpearState throwSpearState = new ThrowSpearState();

    private Animator animator;
    private CharacterController characterController;
    private BoxCollider targetTrigger;

    [HideInInspector]
    public Vector3 moveDirection = new Vector3();
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool TargetFixed;
    [HideInInspector]
    public bool LookingRight = true;
    [HideInInspector]
    public bool ThrowingSpear = false;
    [HideInInspector]
    public bool Aiming = false;
    [HideInInspector]
    public bool isGrabbingLedge = false;

    #endregion

    /*#region RopeMovementFields
    private Rope ropeScript;
    private Rigidbody ropeRigidbody;
    private Collider[] ropeColliders;
    #endregion*/

    #region MethodsForAnimationScripts

    public void Flip()
    {
        if (LookingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        else
        {
           transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

        LookingRight = !LookingRight;
    }
    #endregion

    #region MethodsForAnimationEvents
    public void StandingJump()
    {
        moveDirection.y = jumpForce;
    }
    #endregion

    #region MethodsForCharacterState
    public void SetState(ICharacterState characterState)
    {
        currentState.OnStateExit(this);
        currentState = characterState;
        currentState.OnStateEnter(this);
    }
    #endregion

    public void AdjustPosition(Vector3 offset, Vector3 ledgeOffset, Transform ledge)
    {
        characterController.enabled = false;
        transform.position = transform.position + offset;

        transform.parent = ledge.transform;
        transform.localPosition += ledgeOffset;

        characterController.enabled = true;
    }

    private void Start()
    {
        Target = null;
        TargetFixed = false;

        animator = GetComponent<Animator>();
        animator.SetBool(TransitionParameter.HaveSpear.ToString(), true);

        characterController = GetComponent<CharacterController>();

        targetTrigger = GetComponent<BoxCollider>();
        targetTrigger.enabled = false;

        currentState = Character.freeMoveState;
        currentState.OnStateEnter(this);

        ThrowingSpear = false;

        Debug.Log(currentState);
    }

    private void Update()
    {
        currentState.Update(this);
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

  /*  private void MovementOnRope()
    {
        float forceCoefficient;
        float swingPower = 0.15f;

        // Swinging 
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {

            forceCoefficient = Mathf.Clamp(Vector3.Dot(transform.up, Vector3.right), 0, 1);
            ropeRigidbody.AddForce(swingPower*forceCoefficient*Vector3.right, ForceMode.Impulse);

        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            forceCoefficient = Mathf.Clamp(Vector3.Dot(transform.up, Vector3.left), 0, 1);
            ropeRigidbody.AddForce(swingPower*forceCoefficient * Vector3.left, ForceMode.Impulse);
        }


        //if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        //{
            //transform.position = RopeClimbing(true);
        //}
       // else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
       // {
            //transform.position = RopeClimbing(false);
       // }

        if (Input.GetButtonDown("Jump"))
        {
            //characterController.enabled = true;
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

            characterController.detectCollisions = false;
            currentState = MovementState.JumpOffRope;
            Debug.Log(currentState);

            move_direction.y = jumpForce;
            move_direction.x = horizontal * runSpeed;
            characterController.enabled = true;


        }
    } 
    */

    /*private void JumpOffRope()
    {
       
        move_direction += Physics.gravity * gravityScale * Time.deltaTime;
        characterController.Move(movementOffset + move_direction * Time.deltaTime);
        if (characterController.isGrounded)
        {
            foreach (Collider ropeCollider in ropeColliders)
            {
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ropeCollider, false);
            }
           
            currentState = MovementState.FreeMove;
            Debug.Log(currentState);
        }

        transform.position += move_direction * Time.deltaTime;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(this, other);
    }

    // Move to state 
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


       /* if (hit.gameObject.CompareTag("Rope"))
        {
            if (currentState == MovementState.FreeMove && !characterController.isGrounded)
            {
                characterController.enabled = false;
                transform.SetParent(hit.gameObject.transform);
                ropeScript = hit.gameObject.GetComponent<Rope>();
                ropeRigidbody = hit.gameObject.GetComponent<Rigidbody>();
                ropeColliders = hit.gameObject.transform.parent.gameObject.GetComponentsInChildren<Collider>();

                foreach(Collider ropeCollider in ropeColliders)
                {
                    Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ropeCollider, true);
                }

                currentState = MovementState.Rope;
                Debug.Log(currentState);
            }
            
        } */
    }
}
