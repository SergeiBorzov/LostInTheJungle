using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target; // the object which we will monitor
    public float smoothTime = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}