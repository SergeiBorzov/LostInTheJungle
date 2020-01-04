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


    [SerializeField]
    private Transform eye;
    private List<Transform> possibleTargetList = null;
    private List<Transform> targetList = null;
    private Transform target = null;
    private bool targetFixed = false;
    private int targetIndex = 0;

    public enum TransitionParameter
    {
        Move,
        Turn,
        ForceTransition,
        Jump,
        isGrounded,
        PickSpearThrow,
        SpearThrow,
    }

    public enum MovementState
    {
        FreeMove,
        Rope,
        JumpOffRope,
        Fight
    }

    private Animator animator;
    private CharacterController characterController;
    private BoxCollider targetTrigger;


    private CharacterIK characterIKscript;

    private Rope ropeScript;
    private Rigidbody ropeRigidbody;
    private Collider[] ropeColliders;

    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;

    private MovementState currentState = MovementState.FreeMove;

    void Start() {
        currentState = MovementState.FreeMove;
        Debug.Log(currentState);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        targetTrigger = GetComponent<BoxCollider>();
        targetTrigger.enabled = false;
        possibleTargetList = new List<Transform>();
        targetList = new List<Transform>();
        characterIKscript = GetComponent<CharacterIK>();

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentState = MovementState.Fight;
            animator.SetBool(TransitionParameter.PickSpearThrow.ToString(), true);
            targetTrigger.enabled = true;
            return;
        }



        if (Input.GetKeyDown(KeyCode.E))
        {
            if (targetFixed)
            {
                Debug.Log("Dissolve called!");
                target.GetComponent<Target>().StartDissolve(transform.position + transform.forward + new Vector3(0.0f, 1.0f, 0.0f));
                target = null;
            }
            //currentState = MovementState.FreeMove;
            //ReleaseTargets();
            //targetTrigger.enabled = false;
            //return;
        }







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
        if ( /*!animator.GetCurrentAnimatorStateInfo(0).IsName("RunningJump") && */
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
        float swingPower = 0.15f;

        /* Swinging */
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

    private void JumpOffRope()
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
    }


    private void FindTargets()
    {
        if (possibleTargetList.Count != 0)
        {
            targetList.Clear();
            foreach (Transform possibleTarget in possibleTargetList)
            {
                Vector3 rayDirection = possibleTarget.position - eye.position;
                rayDirection.Normalize();

                if (Vector3.Dot(rayDirection, eye.forward) > 0.0f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(eye.position, rayDirection, out hit, Mathf.Infinity))
                    {
                        // Don't delete useful for debug
                        // Debug.DrawRay(eye.position, rayDirection*10, Color.green);

                        if (hit.transform.gameObject.CompareTag("Target"))
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

    private void ChooseTarget()
    {
        target = targetList[targetIndex];
        target.gameObject.GetComponent<Target>().ChangeMaterial();
        characterIKscript.SetTarget(target.transform);
    }

    private void CheckTarget()
    {
        Vector3 rayDirection = target.position - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Target"))
            {
                return;
            }

            if (!targetFixed)
            {
                Debug.Log("MSG 1");
                targetList.Remove(target);
                target.gameObject.GetComponent<Target>().SetDefaultMaterial();
                target = null;
                targetIndex = 0;
            }
            
        }
        else
        {
            if (!targetFixed)
            {
                Debug.Log("MSG 2");
                targetList.Remove(target);
                target.gameObject.GetComponent<Target>().SetDefaultMaterial();
                target = null;
                targetIndex = 0;
            }
        }

    }

    private void ReleaseTargets()
    {
        if (target != null && !targetFixed)
        {
            target.gameObject.GetComponent<Target>().SetDefaultMaterial();
        }
        possibleTargetList.Clear();
        targetList.Clear();
        //target = null;
    }

    private void FixTarget()
    {
        targetFixed = true;
        animator.SetBool(TransitionParameter.SpearThrow.ToString(), true);
        ReleaseTargets();
        targetTrigger.enabled = false;
        currentState = MovementState.FreeMove;
        //fixedTarget = target;
        //targetList.Remove(fixedTarget);
    }

    private void Fight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentState = MovementState.FreeMove;
            ReleaseTargets();
            animator.SetBool(TransitionParameter.PickSpearThrow.ToString(), false);
            targetTrigger.enabled = false;
            return;
        }

       
        FindTargets();
        Debug.Log("TargetList" + targetList.Count);


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (targetList.Count != 0 && targetList.Count != 1)
            {
                targetIndex = ((targetIndex + 1) % targetList.Count + targetList.Count) % targetList.Count;
                if (target != null)
                {
                    target.GetComponent<Target>().ChangeMaterial();
                }
                ChooseTarget();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (targetList.Count != 0 && targetList.Count != 1)
            {
                targetIndex = ((targetIndex - 1) % targetList.Count + targetList.Count) % targetList.Count;
                if (target != null)
                {
                    target.GetComponent<Target>().ChangeMaterial();
                }
                ChooseTarget();
            }
        }
       

        if (target != null)
        {
            CheckTarget();
        }
        else
        {
            if (targetList.Count != 0)
            {
                ChooseTarget();
            }
        }


        if (Input.GetButtonDown("Jump"))
        {
            FixTarget();
        }

       
        float horizontal_move = Input.GetAxis("Horizontal");
        move_direction.x = horizontal_move * runSpeed;


        if (!characterController.isGrounded)
        {
            animator.SetBool(TransitionParameter.isGrounded.ToString(), false);
            move_direction += Physics.gravity * gravityScale * Time.deltaTime;
        }
        else
        {
            move_direction.y = -9.8f;
        }


        if (Mathf.Abs(transform.position.z) > 0.01f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        characterController.Move(movementOffset + move_direction * Time.deltaTime);
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

            case MovementState.Fight:
            {
                Fight();
                break;
            }
                
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            possibleTargetList.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            possibleTargetList.Remove(other.gameObject.transform);
            targetList.Remove(other.gameObject.transform);
            if (target == other.gameObject.transform)
            {
                other.gameObject.GetComponent<Target>().SetDefaultMaterial();
                target = null;
            }
            targetIndex = 0;

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


        if (hit.gameObject.CompareTag("Rope"))
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
            
        }
    }
}
