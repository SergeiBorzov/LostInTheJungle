using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookReceiver : MonoBehaviour
{
    bool m_Active = false;

    Renderer m_Renderer;
    //SpringJoint m_Joint;
    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    public void Activate(Rigidbody body)
    {
        m_Active = true;
        if (m_Active)
        {
            m_Renderer.material.SetColor("_Color", Color.green);
        }
    }

    public void Deactivate()
    {
        m_Active = false;
        if (!m_Active)
        {
            m_Renderer.material.SetColor("_Color", Color.red);
        }
    }
}
