using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    CharacterController cc;
   
    private Vector3 movement;
    // Physics
    private float acceleration = 1.0f;
    private float decceleration = 1.25f;
    private float max_speed = 8.0f;
    private Vector3 force;
    private Vector3 gravity;

    private float gravity_strength = 1.0f;
   

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Sets movement value to the proper axis
    void OnMove(InputValue movementValue)
    {
        movement.x = movementValue.Get<Vector2>().x;
        movement.z = movementValue.Get<Vector2>().y;
    }

    // Jumping action
    void OnJump(InputValue jumpValue)
    {
        // Only jump if on the ground
        if (cc.isGrounded)
        {
            gravity.y = 20.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Accelerate force , deccelerate force by movement input
        force.x = (movement.x != 0.0f) ? (force.x + movement.x * acceleration) : (force.x / decceleration);
        force.z = (movement.z != 0.0f) ? (force.z + movement.z * acceleration) : (force.z / decceleration);

        // Clamp the force by max speed
        if (force.magnitude > max_speed)
        {
            force = force.normalized * max_speed;
        }

        // If player is not on ground, apply gravity.
        if (!cc.isGrounded)
        {
            gravity += Vector3.down * gravity_strength;
        }
        else
        {
            // If falling and grounded. Set gravity to a small value. The small value lets us make sure we are always colliding with floors.
            if (gravity.y < 0.0f)
            {
                gravity.y = -1.0f;
            }
        }

        // Add all forces together and move the player.
        Vector3 velocity = (force + gravity) * Time.deltaTime;
        //cc.SimpleMove(force);
    }

    // Called when player collides with objects
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
