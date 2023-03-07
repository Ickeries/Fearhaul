using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    private Buoyancy buoyancy;
    private Vector3 movement;
    private Rigidbody rigidbody;

    public float boat_speed = 10.0f;
    public float boat_rotation_speed = 1.0f;
    public float jump_strength = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        buoyancy = GetComponent<Buoyancy>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        movement.x = movementValue.Get<Vector2>().x;
        movement.z = movementValue.Get<Vector2>().y;
    }

    void OnHop()
    {
		if(buoyancy.is_underwater() == true)
		{
			rigidbody.AddForce(new Vector3(0.0f, jump_strength, 0.0f), ForceMode.Impulse);
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculates input direction with respect to camera direction to create a final input
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward_input = forward * movement.z;
        Vector3 right_input = right * movement.x;
        Vector3 final_input = forward_input + right_input;

        // Add forces and rotation to the boat. We lerp the rotation so that we don't snap too quickly to the desired rotation
        if (movement != Vector3.zero)
        {
            Vector3 force_vector = new Vector3(final_input.x, 0.0f, final_input.z);
            Quaternion targetRotation = Quaternion.LookRotation(force_vector);
            Quaternion lerpedRotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, boat_rotation_speed);
            rigidbody.MoveRotation(lerpedRotation);

            if (buoyancy.is_underwater() == true)
            {
                rigidbody.AddForce(transform.forward * boat_speed, ForceMode.Force);
            }
        }

        rigidbody.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
}
