using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update

    enum State
    {
        First,
        Second
    };

    State current = State.First;
    [SerializeField]
    Transform FirstPoint;

    [SerializeField]
    Transform SecondPoint;

    //[SerializeField]
    //float speed = 2.0f;

    float speed = 0.0f;

    private void Start()
    {
        float dist = (FirstPoint.position - SecondPoint.position).magnitude;

        speed = dist / 5.0f;
    }

    void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        switch(current)
        {
            case State.First:
            {
                Vector3 diff = (FirstPoint.position - transform.position);
                dir = diff.normalized;

                float dist = diff.magnitude;

                if (dist < 0.1f)
                {
                    current = State.Second;
                }
                break;
            }
            case State.Second:
            {
                Vector3 diff = (SecondPoint.position - transform.position);
                dir = diff.normalized;

                float dist = diff.magnitude;

                if (dist < 0.1f)
                {
                    current = State.First;
                }
                break;
            }
        }

        transform.position += dir * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            float diff_y = transform.position.y - character.transform.position.y;

            if (diff_y > 0.0f && character.isGrounded)
            {
                //character.isDead = true;
                character.Die();
            }

            character.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            character.transform.parent = null;
        }
    }
}
