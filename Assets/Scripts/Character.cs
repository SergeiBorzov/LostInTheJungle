using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 move_direction = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float gravityScale = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        move_direction = new Vector3(Input.GetAxis("Horizontal")*speed, move_direction.y, move_direction.z);

       
        if (characterController.isGrounded) {
            if(Input.GetButtonDown("Jump")) {
                move_direction.y = jumpForce;
            }
        }

        move_direction.y += (Physics.gravity.y * gravityScale * Time.deltaTime);

        if (transform.position.z != 0)
        {
            movementOffset.z = (0 - transform.position.z) * 0.1f;
        }

        characterController.Move(movementOffset);
        characterController.Move(move_direction*Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }

}