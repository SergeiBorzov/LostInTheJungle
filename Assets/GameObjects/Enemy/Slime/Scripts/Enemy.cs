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



    private bool knockBack = false;
    private Vector3 knockBackDirection = Vector3.zero;
    Renderer slimeRenderer;


    void Start()
    {
        
        currentState = State.Patrol;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        slimeRenderer = GetComponentInChildren<Renderer>();
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
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
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
            else
            {
                animator.SetTrigger("Damage");
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
    void Update()
    {
        float dist = (transform.position - playerTransform.position).magnitude;


        if (dead)
        {
            return;
        }

        switch(currentState)
        {
            case State.Patrol:
            {
                float dist1 = (transform.position - patrolPointFirst.position).magnitude;
                float dist2 = (transform.position - patrolPointSecond.position).magnitude;


                //Debug.Log("Patrol");
                if (dist < tauntRadius)
                {
                    currentState = State.Follow;
                    agent.isStopped = false;
                    return;
                }
                else if (dist1 < attackRadius)
                {
                    agent.SetDestination(patrolPointSecond.position);
                }
                else if (dist2 < attackRadius)
                {
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
                //Debug.Log("Attack");
                
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
