using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HookReceiver : MonoBehaviour
{
    [SerializeField]
    Material active;

    [SerializeField]
    Material notActive;

    Light m_Light;

    bool m_Active = false;

    Renderer m_Renderer;
    //SpringJoint m_Joint;
    private void Start()
    {
        m_Light = GetComponentInChildren<Light>();
        m_Light.enabled = true;
        m_Renderer = GetComponent<Renderer>();
    }

    public void Activate(Rigidbody body)
    {
        //m_Active = true;
        //if (m_Active)
        //{
        //    m_Renderer.material = notActive;
        //}
    }

    public void Deactivate()
    {
        //m_Active = false;
        //if (!m_Active)
        //{
        //    m_Renderer.material = active;
        //}
    }
}
