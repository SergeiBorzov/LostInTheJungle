using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target; // the object which we will monitor
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    /*
    private void Start()
    {
        offset = transform.position - target.transform.position;
    }
    */

    void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
        transform.LookAt(target);
    }
}