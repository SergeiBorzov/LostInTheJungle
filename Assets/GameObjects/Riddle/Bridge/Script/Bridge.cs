using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    Animator m_Animator;
    private bool m_Open = false;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Action()
    {
        m_Animator.SetBool("Open", !m_Open);
        m_Open = !m_Open;
    }

}
