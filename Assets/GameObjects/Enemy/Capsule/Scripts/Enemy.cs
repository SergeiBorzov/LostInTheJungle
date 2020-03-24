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
    private float followDistance = 10.0f;

    [SerializeField]
    private int hp = 3;

    private bool dead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }


    private void Die()
    {
        dead = true;
        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        gameObject.layer = LayerMask.NameToLayer("Fracture");

        Destroy(gameObject, 5.0f);
    }

    public void TakeDamage()
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

    void Update()
    {
        float dist = (transform.position - playerTransform.position).magnitude;
        if ( dist > followDistance && !dead)
        {
            agent.enabled = true;
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.enabled = false;
        }
    }
}
