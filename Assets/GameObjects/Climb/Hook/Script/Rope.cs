using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private List<Transform> m_Receivers = new List<Transform>();
    private CharacterController m_CharacterController;
    private Character m_CharacterScript;
    private LineRenderer m_LineRenderer;

    [SerializeField]
    private Transform m_GraplePoint;

    Vector3 m_ReceiverPosition;

    Vector3 m_Velocity;

    private void Start()
    {
        m_CharacterController = GetComponentInParent<CharacterController>();
        m_CharacterScript = GetComponentInParent<Character>();
        m_LineRenderer = GetComponent<LineRenderer>();

        m_LineRenderer.positionCount = 2;
    }

    private void Graple()
    {
        if (m_Receivers.Count > 0)
        {
            m_CharacterScript.isHook = true;
            m_ReceiverPosition = m_Receivers[0].position;

            m_Velocity = m_CharacterController.velocity;
        }
    }

    private void Move()
    {
        Vector3 n = m_ReceiverPosition - m_CharacterScript.transform.position;
        n = new Vector3(n.x, n.y, 0.0f);
        n.Normalize();

        float cos = Vector3.Dot(n, Vector3.right);

        float angle = Mathf.Rad2Deg * Mathf.Acos(cos);
        m_CharacterController.Move(m_Velocity);    
    }

    private void Degraple()
    {
        m_CharacterScript.isHook = false;
    }

    private void DrawRope()
    {
        m_LineRenderer.SetPosition(0, m_CharacterScript.transform.position);
        m_LineRenderer.SetPosition(1, m_ReceiverPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Graple();
        }

        if (m_CharacterScript.isHook)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Degraple();
            }
        }
    }

    private void LateUpdate()
    {
        if (m_CharacterScript.isHook)
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
