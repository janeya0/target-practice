using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12;
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    public float jumpHeight = 3f;

    // Update is called once per frame
    void Update()
    {
        if (!MouseLook.gamePaused) {
            // Checks if player is grounded using CheckSphere tool
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Perform jump only when space is pressed and player is grounded
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Get X and Z axis values 
            float x = Input.GetAxis("Horizontal"); 
            float z = Input.GetAxis("Vertical"); 

            // Side to side movement (arrow keys)
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            // Vertical movement (implementing gravity)
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }
}
