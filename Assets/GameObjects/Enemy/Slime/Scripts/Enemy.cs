using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float attackRadius = 1.5f;
    [SerializeField]
    private float tauntRadius = 3.0f;
    [SerializeField]
    private float followRadius = 10.0f;
    [SerializeField]
    private int hp = 3;
    [SerializeField]
    Transform patrolPointFirst;
    [SerializeField]
    Transform patrolPointSecond;
    [SerializeField]
    float knockBackForce = 2.0f;

    private bool dead = false;

    private enum State {Follow, Patrol, Attack}

    private State currentState;

    private bool AttackReady = true;
    private float AttackWait = 3.0f;
    private bool hitted = false;



    private bool knockBack = false;
    private Vector3 knockBackDirection = Vector3.zero;
    Renderer slimeRenderer;

    MeshCollider m_Collider;

    [SerializeField]
    BoxCollider m_LeftTrigger;
    [SerializeField]
    BoxCollider m_RightTrigger;

    void Start()
    {
        
        currentState = State.Patrol;
        animator = GetComponent<Animator>();
        animator.SetBool("Move", true);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        slimeRenderer = GetComponentInChildren<Renderer>();
        m_Collider = GetComponentInChildren<MeshCollider>();
        m_LeftTrigger.enabled = false;
        m_RightTrigger.enabled = false;
       
        if (slimeRenderer == null)
        {
            Debug.Log("Slime Renderer is null!!!!");
        }
        agent.updateRotation = false;
        agent.SetDestination(patrolPointFirst.position);
    }

    private void FixedUpdate()
    {
        if (knockBack)
        {
            agent.velocity = knockBackDirection * knockBackForce;
        }
    }

    private void Die()
    {
        dead = true;
        agent.enabled = false;

        rb.isKinematic = true;
        m_Collider.enabled = false;
        
        
        gameObject.layer = LayerMask.NameToLayer("Fracture");
        animator.SetTrigger("Death");
        Destroy(gameObject, 5.0f);
    }

    IEnumerator ChangeColor()
    {
        slimeRenderer.material.SetColor("_Color", new Color(0.9f, 0.0f, 0.0f));
        yield return new WaitForSeconds(0.2f);
        slimeRenderer.material.SetColor("_Color", Color.white);
    }

    IEnumerator KnockBack()
    {
        knockBack = true;
        agent.speed = 10;
        agent.angularSpeed = 0;
        agent.acceleration = 20;
        slimeRenderer.material.SetColor("_Color", new Color(0.9f, 0.0f, 0.0f));

        yield return new WaitForSeconds(0.2f);

        knockBack = false;
        agent.speed = 4;
        agent.angularSpeed = 180;
        agent.acceleration = 10;
    }

    private void TakeDamage()
    {
        if (!dead)
        {
            StartCoroutine(ChangeColor());
            hp--;
            if (hp == 0)
            {
                Die();
            }
        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        Sword swordScript = other.gameObject.GetComponent<Sword>();

        if (swordScript != null)
        {
            TakeDamage();
            if (!dead)
            {
                knockBackDirection = playerTransform.forward;
                StartCoroutine(KnockBack());
            }
        }

       

       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!hitted)
            {
                var character = other.gameObject.GetComponent<Character>();

                character.TakeDamage(22.0f);
                character.isAttackedFromRight = (character.transform.position - transform.position).x < 0.0f;
                character.OnAttack();
                hitted = true;
            }
        }
    }

    private void LeftAnimationEventStart()
    {
        m_LeftTrigger.enabled = true;
    }

    private void LeftAnimationEventEnd()
    {
        m_LeftTrigger.enabled = false;
    }

    private void RightAnimationEventStart()
    {
        m_RightTrigger.enabled = true;
    }

    private void RightAnimationEventEnd()
    {
        m_RightTrigger.enabled = false;
    }

    private void Update()
    {
        Vector3 diff = playerTransform.position - transform.position;

       
        float dist = diff.magnitude;


        if (dead)
        {
            return;
        }

        switch(currentState)
        {
            case State.Patrol:
            {
                Vector3 diff1 = patrolPointFirst.position - transform.position;
                float dist1 = diff1.magnitude;

                Vector3 diff2 = patrolPointFirst.position - transform.position;
                float dist2 = diff2.magnitude;


                //Debug.Log("Patrol");
                if (dist < tauntRadius)
                {
                    if (diff.x > 0)
                    {
                        animator.SetBool("LookingRight", true);
                    }
                    else
                    {
                        animator.SetBool("LookingRight", false);
                    }
                    currentState = State.Follow;
                    agent.isStopped = false;
                    return;
                }
                else if (dist1 < attackRadius)
                {
                    if (diff1.x > 0)
                    {
                        animator.SetBool("LookingRight", true);
                    }
                    else
                    {
                        animator.SetBool("LookingRight", false);
                    }
                    agent.SetDestination(patrolPointSecond.position);
                }
                else if (dist2 < attackRadius)
                {
                    if (diff2.x > 0)
                    {
                        animator.SetBool("LookingRight", true);
                    }
                    else
                    {
                        animator.SetBool("LookingRight", false);
                    }
                    agent.SetDestination(patrolPointFirst.position);
                }
                break;
            }
            case State.Follow:
            {
                //Debug.Log("Follow");
                agent.SetDestination(playerTransform.position);

                if (dist < attackRadius)
                {
                    currentState = State.Attack;
                    agent.isStopped = true;
                }
                else if (dist > followRadius)
                {
                    currentState = State.Patrol;
                    agent.isStopped = true;
                }
                break;
            }
            case State.Attack:
            {
                if (AttackReady)
                {
                    animator.SetTrigger("Attack");
                    AttackWait = 3.0f;
                    AttackReady = false;
                }

                AttackWait -= Time.deltaTime;
                if (AttackWait < 0.0f)
                {
                    AttackReady = true;
                    hitted = false;
                }


                if (dist > attackRadius)
                {
                    currentState = State.Follow;
                    agent.isStopped = false;
                }

                break;
            }
        }
    }
}
