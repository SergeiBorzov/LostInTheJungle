using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    void Update()
    {
        VirtualInputManager.Instance.MoveRight = Input.GetKey(KeyCode.D);
        VirtualInputManager.Instance.MoveLeft = Input.GetKey(KeyCode.A);
        VirtualInputManager.Instance.Run = Input.GetKey(KeyCode.LeftShift);
        VirtualInputManager.Instance.Jump = Input.GetKey(KeyCode.Space);
    }
}

