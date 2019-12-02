using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            VirtualInputManager.Instance.MoveRight = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            VirtualInputManager.Instance.MoveLeft = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveLeft = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            VirtualInputManager.Instance.Run = true;
        }
        else
        {
            VirtualInputManager.Instance.Run = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            VirtualInputManager.Instance.Jump = true;
        }
        else
        {
            VirtualInputManager.Instance.Jump = false;
        }
    }
}

