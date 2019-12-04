using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    private Animator animator;
    private CharacterController characterController;


    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);

    /* Useful flags */
    private bool lookingRight = true;

   

    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    private void Flip()
    {
        if (lookingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        else
        {
           transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

        lookingRight = !lookingRight;
    }

    void Update() {

        float horizontal_move = Input.GetAxis("Horizontal");
        move_direction.x = horizontal_move * runSpeed;
        move_direction += Physics.gravity * gravityScale * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            move_direction.y = 0.0f;
            move_direction.y = jumpForce;
        }

        if (horizontal_move > 0.0f && !lookingRight)
        {
            Flip();
        }

        if (horizontal_move < 0.0f && lookingRight)
        {
            Flip();
        }

        if (transform.position.z != 0.0f)
        {
            movementOffset.z = (0.0f - transform.position.z) * 0.1f;
        }

        characterController.Move(movementOffset + move_direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}