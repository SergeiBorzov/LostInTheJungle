using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;

    private Rigidbody rigidBody;
  

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector3(horizontal * playerSpeed, rigidBody.velocity.y, 0.0f);
    }

    private void Update()
    {
        

    }
}
