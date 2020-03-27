using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float attackRadius = 1.5f;

    [SerializeField]
    private float followRadius = 10.0f;

    [SerializeField]
    private int hp = 3;

    private bool dead = false;

    private enum State {Follow, Patrol, Attack}

    private State currentState;
    void Start()
    {
        
        currentState = State.Patrol;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }


    private void Die()
    {
        dead = true;
        agent.enabled = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        gameObject.layer = LayerMask.NameToLayer("Fracture");

        Destroy(gameObject, 5.0f);
    }

    private void TakeDamage()
    {
        if (!dead)
        {
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
                Debug.Log("Patrol");
                if (dist < followRadius)
                {
                    currentState = State.Follow;
                    agent.isStopped = false;
                }
                break;
            }
            case State.Follow:
            {
                Debug.Log("Follow");
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
                Debug.Log("Attack");
                
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
