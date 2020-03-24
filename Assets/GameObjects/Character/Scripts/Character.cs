using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICharacterState
{
    void OnStateEnter(GameObject character);
    void Update();
    void OnStateExit();
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}

public class Character : MonoBehaviour
{
    private ICharacterState currentState;


    #region EditorVariables
    [SerializeField] public float heightPadding = 0.05f;
    #endregion

    #region TransitionParameters
    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        Jump,
        RunningJump,
        isGrounded,
        isGrabbingLedge,
        Climb,
        Falling,
        LandingNeeded,
        HangUp,
        HangDown,
        Fight,
        FightEnd
    }
    #endregion


    #region Physics
    [SerializeField] public float gravityScale = 3.0f;

    public Vector3 verticalVelocity = Vector3.zero;
    private Vector3 hitNormal;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public float groundAngle;
    [HideInInspector] public float maxGroundAngle = 120.0f;
    [HideInInspector] public bool isGrounded = false;

    private Vector3 centeredPosition;

    [SerializeField] public float jumpForce = 10.0f;

    [HideInInspector] public int clicks = 0;





    [HideInInspector]
    public bool gravityOn = true;
    [HideInInspector]
    public bool moveOn = true;
    [HideInInspector]
    public bool isIdle = true;
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool lookingRight = true;
    [HideInInspector]
    public bool isLanding = false;
    [HideInInspector]
    public bool isTurning = false;
    //[HideInInspector]
    public bool isFight = false;
    //[HideInInspector]
    public bool isFightEnd = false;
    [HideInInspector]
    public bool isGrabbingLedge = false;
    [HideInInspector]
    public bool isJumping = false;
    [HideInInspector]
    public bool isClimbing = false;
    [HideInInspector]
    public bool isDroping = false;
    [HideInInspector]
    public bool isHangJumping = false;
    [HideInInspector]
    public bool isFalling = false;
    
    #endregion


    #region Components
    private CharacterController characterController;
    private Animator animator;

    public Ledge grabbedLedge;

    public Sword swordScript;
    public Collider swordCollider;

    [SerializeField]
    private ParticleSystem trail;

    [SerializeField]
    private ParticleSystem trail2;

    [SerializeField]
    private ParticleSystem trail3;
    #endregion




    private void EventTrailOn()
    {
        trail.Play();
        swordCollider.enabled = true;
        swordScript.canDamage = true;
    }

    private void EventTrailOff()
    {
        trail.Stop();
        swordCollider.enabled = false;
    }

    private void EventTrail2On()
    {
        trail2.Play();
        swordCollider.enabled = true;
        swordScript.canDamage = true;
    }

    private void EventTrail2Off()
    {
        trail2.Stop();
        swordCollider.enabled = false;
    }

    private void EventTrail3On()
    {
        trail3.Play();
        swordCollider.enabled = true;
        swordScript.canDamage = true;
    }

    private void EventTrail3Off()
    {
        trail3.Stop();
        swordCollider.enabled = false;
    }




    public bool IsClimbLedge()
    {
        return grabbedLedge.next == null;
    }

    public bool IsDropLedge()
    {
        return grabbedLedge.previous == null;
    }

    public void SetAnimatorHanging()
    {
        animator.SetBool(Character.TransitionParameter.isGrabbingLedge.ToString(), true);
    }

    public void StandingJump()
    {
        gravityOn = false;
        verticalVelocity.y = jumpForce;
    }

    public void HangJump()
    {
        isGrabbingLedge = false;
        moveOn = true;
        verticalVelocity.y = jumpForce;
        transform.parent = null;
    }

    public void PerformClimb()
    {
        characterController.enabled = false;
        transform.position = grabbedLedge.transform.position + grabbedLedge.endPoint;
        characterController.enabled = true;
        transform.parent = null;
    }

    public void PerformHangUp()
    {
        characterController.enabled = false;
        grabbedLedge = grabbedLedge.next.gameObject.GetComponent<Ledge>();
        transform.position += grabbedLedge.hangOffset;
        transform.parent = grabbedLedge.transform;
        characterController.enabled = true;
    }

    public void PerformHangDown()
    {
        characterController.enabled = false;
        grabbedLedge = grabbedLedge.previous.gameObject.GetComponent<Ledge>();

        Vector3 offset = grabbedLedge.hangOffset;
        offset.y = -offset.y;
        transform.position += offset;
        transform.parent = grabbedLedge.transform;
        characterController.enabled = true;
    }

    public void AdjustPosition(Vector3 offset, Vector3 ledgeOffset, Transform ledge)
    {
        characterController.enabled = false;
        transform.position = transform.position + offset;

        transform.parent = ledge.transform;
        transform.localPosition += ledgeOffset;

        characterController.enabled = true;
    }

    public void Flip()
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


    // Recalculate forward vector, to fight jittering on terrain slopes
    private void CalculateForward()
    {
        if (!isGrounded)
        {
            forward = transform.forward;
        }
        else
        {
            forward = Vector3.Cross(transform.right, hitNormal);
        }
    }

    private void CalculateGroundAngle()
    {
        if (isGrounded)
        {
            groundAngle = 90.0f;
            return;
        }

        groundAngle = Vector3.Angle(hitNormal, transform.forward);
    }

    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(centeredPosition, Vector3.down, out hit, characterController.height / 2.0f + heightPadding, 1 << 9))
        {
            isGrounded = true;
            hitNormal = hit.normal;
        }
        else
        {
            hitNormal = Vector3.up;
            isGrounded = characterController.isGrounded;
        }

        animator.SetBool(Character.TransitionParameter.isGrounded.ToString(), isGrounded);

    }

    public void SetState(ICharacterState characterState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = characterState;
        currentState.OnStateEnter(gameObject);
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isTurning = false;
        isLanding = false;

        swordScript = GetComponentInChildren<Sword>();
        swordCollider = swordScript.GetComponent<Collider>();
        centeredPosition = transform.position + new Vector3(0.0f, characterController.height / 2.0f, 0.0f);
        SetState(new FreeMoveState());
        trail.Stop();
        trail2.Stop();
        trail3.Stop();
    }

    private void ApplyGravity()
    {
        if (gravityOn && !isGrounded)
        {
            verticalVelocity.y += (Physics.gravity * gravityScale * Time.deltaTime).y;
        }
    }

    private void FightJittering()
    {
        characterController.Move(new Vector3(0.0f, verticalVelocity.y, 0.0f) * Time.deltaTime);
    }
    void Update()
    {
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();

        currentState.Update();

        ApplyGravity();
        FightJittering();
    }


    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }
        else
        {
            Enemy enemyScript = body.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                return;
            }


            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // If you know how fast your character is trying to move,
            // then you can also multiply the push velocity by that.

            // Apply the push
            body.velocity = pushDir * 5.0f;
        }
    }





}
