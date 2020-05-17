using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    [SerializeField]
    Character m_CharacterScript;

    public bool m_canPush = true;
    public bool m_canPull = true;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        var character = other.GetComponent<Character>();

        if (character != null)
        {
           
            if (character.lookingRight)
            {
                float dot = Vector3.Dot(Vector3.right, transform.position - character.transform.position);
                if (dot > 0.0f)
                {
                    character.grabbedBox = this;
                    character.isNearBox = true;
                }
                else
                {
                    character.grabbedBox = null;
                    character.isNearBox = false;
                }
            }
            else
            {
                float dot = Vector3.Dot(Vector3.left, transform.position - character.transform.position);
                if (dot > 0.0f)
                {
                    character.grabbedBox = this;
                    character.isNearBox = true;
                }
                else
                {
                    character.grabbedBox = null;
                    character.isNearBox = false;
                }
            }
            
        }
    }

    public void Grab()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        //m_Rigidbody.isKinematic = true;
        transform.parent = m_CharacterScript.transform;
    }
    public void Release()
    {
        transform.parent = null;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        //m_Rigidbody.isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        var character = other.GetComponent<Character>();

        if (character != null)
        {
            character.grabbedBox = null;
            character.isNearBox = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            if (m_CharacterScript.isPushing)
            {
                m_canPush = false;
            }
            else if (m_CharacterScript.isPulling)
            {
                m_canPull = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            m_canPush = true;
            m_canPull = true;
        }
    }

}
