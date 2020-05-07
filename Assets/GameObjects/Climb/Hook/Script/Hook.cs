﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    List<Transform> m_Receivers = new List<Transform>();

    Rigidbody m_Rigidbody;
    CharacterIK m_CharacterIK;
    Character m_CharacterScript;
    CharacterController m_CharacterController;
    LineRenderer m_LineRenderer;
    SpringJoint m_Joint;
    Animator m_Animator;

    public bool m_SwingReady = false;
    bool m_Swing_Right = false;
    bool m_Swing_Left = false;

    bool m_Grapple = false;
    bool m_Degrapple = false;

    bool m_Up = false;
    bool m_Down = false;

    Vector3 m_ReceiverPosition;

    [SerializeField] 
    private float m_SwingSpeed = 8.0f;

    [SerializeField]
    private Transform m_Graple;

    [SerializeField]
    float m_SwingCos = 0.8f;

    [SerializeField]
    float m_Friction = 3.0f;

    public bool flip = false;
    public Vector3 GetReceiver()
    {
        return m_ReceiverPosition;
    }

    private void Start()
    {
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_CharacterController = GetComponentInParent<CharacterController>();
        m_CharacterScript = GetComponentInParent<Character>();
        m_CharacterIK = GetComponentInParent<CharacterIK>();
        m_Joint = GetComponentInParent<SpringJoint>();
        m_LineRenderer = GetComponent<LineRenderer>();
        m_Animator = GetComponentInParent<Animator>();

        m_Joint.maxDistance = 0.0f;
        m_Joint.minDistance = 4.0f;

        m_Joint.autoConfigureConnectedAnchor = false;
        m_Joint.anchor = new Vector3(-0.25f, 2.03f, -0.07f);
        m_Joint.spring = 30.5f;
        m_Joint.damper = 100.0f;
        m_Joint.massScale = 1.5f;


        m_LineRenderer.positionCount = 2;
        m_LineRenderer.enabled = false;
    }

    public void Grapple()
    {
        m_Joint.connectedAnchor = m_Receivers[0].position;

        float distanceFromPoint = Vector3.Distance(m_Graple.position, m_Receivers[0].gameObject.transform.position);
       
        m_CharacterScript.isHook = true;
        m_CharacterScript.verticalVelocity = Vector3.zero;
        m_CharacterController.enabled = false;
        m_CharacterScript.HookColliderOn();

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = 1.25f*m_CharacterScript.GetVelocity();

        m_LineRenderer.enabled = true;

        m_Animator.SetBool(Character.TransitionParameter.Hook.ToString(), true);
    }

    public void Degrapple()
    {
        m_CharacterScript.isHook = false;
        m_Joint.connectedBody = null;

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.isKinematic = true;

        m_CharacterScript.HookColliderOff();
        Quaternion q = m_CharacterScript.transform.rotation;
        m_CharacterScript.transform.rotation = Quaternion.Euler(0.0f, q.eulerAngles.y, q.eulerAngles.z);
        m_CharacterController.enabled = true;
        m_CharacterScript.isHook = false;

        m_LineRenderer.enabled = false;

        m_Animator.SetBool(Character.TransitionParameter.Hook.ToString(), false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !m_CharacterScript.isHook && !m_CharacterScript.isGrounded) {
            if (m_Receivers.Count != 0)
            {
                m_Grapple = true;
                /*var script = m_Receivers[0].GetComponent<HookReceiver>();
                m_ReceiverPosition = m_Receivers[0].position;
                Grapple();
                script.Activate(m_Rigidbody);*/
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_CharacterScript.isHook)
        {
            if (m_Receivers.Count != 0)
            {
                m_Degrapple = true;
                /*var script = m_Receivers[0].GetComponent<HookReceiver>();
                Degrapple();
                script.Deactivate();
                m_CharacterScript.HookJump();*/
            }
        }

        
        if (Input.GetKey(KeyCode.A) )
        {
            if (m_CharacterScript.lookingRight && !m_CharacterScript.isTurning)
            {
               
                m_Animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
                
            }
            m_Swing_Left = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!m_CharacterScript.lookingRight && !m_CharacterScript.isTurning)
            {
                m_Animator.SetBool(Character.TransitionParameter.Turn.ToString(), true);
            }
            m_Swing_Right = true;
        }

        /*if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            m_SwingReady = true;
        }*/

        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            m_Up = true;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            m_Down = true;
        }
    }


    private void Stop()
    {
        Vector3 n = m_Receivers[0].position - m_Graple.position;
        n = new Vector3(n.x, n.y, 0.0f);
        n.Normalize();
        float cos = Vector3.Dot(n, Vector3.right);
        float s = m_Rigidbody.velocity.magnitude;
        m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * (s - Time.fixedDeltaTime * m_Friction * Mathf.Abs(cos));
    }
    private void FixedUpdate()
    {
       
        if (m_Grapple)
        {
            m_Grapple = false;
            var script = m_Receivers[0].GetComponent<HookReceiver>();
            m_ReceiverPosition = m_Receivers[0].position;
            if (m_Graple.position.y < m_ReceiverPosition.y)
            {
                Grapple();
                script.Activate(m_Rigidbody);
            }
        }
        else if (m_Degrapple)
        {
            m_Joint.minDistance = 4.0f;
            m_Degrapple = false;
            var script = m_Receivers[0].GetComponent<HookReceiver>();
            Degrapple();
            script.Deactivate();
            m_CharacterScript.HookJump();
        }



        if (m_CharacterScript.isHook && m_Receivers.Count > 0)
        {
            Vector3 n = m_Receivers[0].position - m_Graple.position;
            n = new Vector3(n.x, n.y, 0.0f);
            n.Normalize();

            float cos = Vector3.Dot(n, Vector3.right);

            if (m_Swing_Right)
            {
                Debug.Log("Here");
                if (cos > 0.0f)
                {
                    m_Rigidbody.velocity += new Vector3(n.y, -n.x, 0.0f) * Time.fixedDeltaTime * 2.0f;
                }
                else
                {
                    Debug.Log("Stop");
                    Stop();
                }
                m_Swing_Right = false;
            }
            else if (m_Swing_Left)
            {
                if (cos < 0.0f)
                {
                    m_Rigidbody.velocity += new Vector3(-n.y, n.x, 0.0f) * Time.fixedDeltaTime * 2.0f;
                    
                }
                else
                {
                    Stop();
                }
                m_Swing_Left = false;
            }
            else
            {
                //Stop();
            }

            float speed = m_Rigidbody.velocity.magnitude;
            if (speed > m_SwingSpeed)
            {
                m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_SwingSpeed;
            }
        }


        if (m_Up)
        {
            if (m_Joint.minDistance < 5.0f)
            {
                m_Joint.minDistance += Time.fixedDeltaTime * 8.0f;
                if (m_Joint.minDistance > 5.0f)
                {
                    m_Joint.minDistance = 5.0f;
                }
            }
            m_Up = false;
        }   
        else if (m_Down)
        {
            if (m_Joint.minDistance > 1.0f)
            {
                m_Joint.minDistance -= Time.fixedDeltaTime * 8.0f;
                if (m_Joint.minDistance < 1.0f)
                {
                    m_Joint.minDistance = 1.0f;
                }
            }
            m_Down = false;
        }

        
    }

    private void DrawRope()
    {
        m_LineRenderer.SetPosition(0, m_Graple.position);
        m_LineRenderer.SetPosition(1, m_ReceiverPosition);
    }
    private void LateUpdate()
    {
        if (flip)
        {
            flip = false;
            Vector3 v = m_Rigidbody.velocity;
            m_Rigidbody.isKinematic = true;
            m_CharacterScript.Flip();
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.velocity = v;
        }


        Vector3 n = m_Receivers[0].position - m_Graple.position;
        n = new Vector3(n.x, n.y, 0.0f);
        n.Normalize();

        if (m_CharacterScript.isHook)
        {
            float cos = Vector3.Dot(n, Vector3.right);
            Quaternion quat = m_CharacterScript.transform.rotation;
            if (m_CharacterScript.lookingRight)
            {
                Quaternion res = Quaternion.Euler(90.0f - Mathf.Rad2Deg * Mathf.Acos(cos), quat.eulerAngles.y, quat.eulerAngles.z);
                Quaternion.Lerp(quat, res, Time.deltaTime*2.0f);
            }
            else
            {
                Quaternion res = Quaternion.Euler(-90.0f + Mathf.Rad2Deg * Mathf.Acos(cos), quat.eulerAngles.y, quat.eulerAngles.z);
                Quaternion.Lerp(quat, res, Time.deltaTime * 2.0f);
            }
        }
        

        if (m_LineRenderer.enabled)
        {
            DrawRope();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HookReceiver script = other.gameObject.GetComponent<HookReceiver>();
        if (script != null)
        {
            if (other.transform.position.y > transform.position.y)
            {
                m_Receivers.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HookReceiver script = other.gameObject.GetComponent<HookReceiver>();
        if (script != null)
        {
            m_Receivers.Remove(other.transform);
        }
    }
}