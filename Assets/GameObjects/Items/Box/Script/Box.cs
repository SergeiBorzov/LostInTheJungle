﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    BoxCollider m_BoxCollider;
    SphereCollider m_SphereCollider;

    [SerializeField]
    Character m_CharacterScript;

    public bool m_canPush = true;
    public bool m_canPull = true;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.enabled = true;

        m_SphereCollider = GetComponent<SphereCollider>();
        m_SphereCollider.enabled = false;
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
        transform.parent = m_CharacterScript.transform;
        m_Rigidbody.isKinematic = true;
        m_BoxCollider.enabled = false;
        m_SphereCollider.enabled = true;
        m_Rigidbody.isKinematic = false;
    }

    public void Release()
    {
        transform.parent = null;
        m_Rigidbody.isKinematic = true;
        m_BoxCollider.enabled = true;
        m_SphereCollider.enabled = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        /*var character = other.GetComponent<Character>();

        if (character != null)
        {
            character.grabbedBox = null;
            character.isNearBox = false;
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground") && 
            collision.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (m_CharacterScript && m_CharacterScript.isPushing)
            {
                m_canPush = false;
            }
            else if (m_CharacterScript && m_CharacterScript.isPulling)
            {
                m_canPull = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground") &&
            collision.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            m_canPush = true;
            m_canPull = true;
        }
    }

}
