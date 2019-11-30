using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target; // the object which we will monitor
    public float camera_speed = 5.0f;
    public Vector3 offset;

    /*
    private void Start()
    {
        offset = transform.position - target.transform.position;
    }
    */

    void Update()
    {
        /*
        transform.LookAt(target); // change the direction of the camera view
        Vector3 move_direction = target.position - transform.position;
        move_direction.y = 0;
        move_direction.z = 0;
        // if (move_direction.magnitude > 0.1f) {
        //move_direction = Vector3.Normalize(move_direction);
        transform.position += move_direction * Time.deltaTime * camera_speed;
        //}
        */
        transform.LookAt(target);
        transform.position = target.transform.position + offset;
    }
}