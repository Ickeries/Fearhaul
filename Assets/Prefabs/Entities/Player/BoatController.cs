using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class BoatController : MonoBehaviour
{
    private Buoyancy buoyancy;
    private Rigidbody rigidbody;

    // Publics
    public float boat_speed = 10.0f;
    public float boat_rotation_speed = 1.0f;
    public float jump_strength = 10.0f;
    public float jump_height = 10.0f;
    public float jump_time_to_peak = 1.0f;
    public float jump_time_to_descent = 1.0f;

    public TMP_Text currentStateText;

    private float jump_velocity = 0.0f;
    private float jump_gravity = 0.0f;
    private float fall_gravity = 0.0f;
    private Vector3 movement;


    enum STATES { Idle, Moving, Charge, Aim}
    private STATES state = STATES.Idle;

    //Input
    PlayerInput playerInput;
    InputAction JumpActions;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        JumpActions = playerInput.actions["Jump"];
        buoyancy = GetComponent<Buoyancy>();
        rigidbody = GetComponent<Rigidbody>();
        update_jump_velocity();
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
        Vector3 movement_vector = get_movement_vector();
        currentStateText.text = System.Enum.GetName(typeof(STATES), state);
        switch (state)
        { 
        case STATES.Idle:
                if (playerInput.actions["Jump"].WasPressedThisFrame())
                {
                    rigidbody.AddForce(new Vector3(0.0f, jump_strength, 0.0f), ForceMode.Impulse);
                }
                else if (movement_vector.sqrMagnitude > 0.0f)
                {
                    enter_state(STATES.Moving);
                }
                break;
        case STATES.Moving:
                if (playerInput.actions["Jump"].WasPressedThisFrame())
                {
                    rigidbody.AddForce(new Vector3(0.0f, jump_strength, 0.0f), ForceMode.Impulse);
                }
                else if (movement_vector.sqrMagnitude == 0.0f)
                {
                    enter_state(STATES.Idle);
                }
                else if (playerInput.actions["Charge"].IsPressed())
                {
                    enter_state(STATES.Charge);    
                }
                else
                {
                    // Move boat in direction of movement vector
                    rotateRigidBodyToVector(movement_vector, 1.0f);
                    rigidbody.AddForce(transform.forward * boat_speed, ForceMode.Force);
                }
                break;
        case STATES.Charge:
                if (movement_vector.sqrMagnitude == 0.0f)
                {
                    enter_state(STATES.Idle);
                }
                else if (!playerInput.actions["Charge"].IsPressed())
                {
                    enter_state(STATES.Moving);
                }
                rotateRigidBodyToVector(movement_vector, 0.2f);
                rigidbody.AddForce(transform.forward * boat_speed * 1.25f, ForceMode.Force);
                break;
        }
        rigidbody.AddForce(Physics.gravity * 4.0f, ForceMode.Acceleration);
        if (rigidbody.velocity.magnitude > 100.0f)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * 100.0f;
        }
    }

    void update_jump_velocity()
    {
        jump_velocity = (2.0f * jump_height) / jump_time_to_peak;
        jump_gravity = (-2.0f * jump_height) / (jump_time_to_peak * jump_time_to_peak);
        fall_gravity = (-2.0f * jump_height) / (jump_time_to_descent * jump_time_to_peak);
    }

    private Vector3 get_movement_vector()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward_input = forward * movement.z;
        Vector3 right_input = right * movement.x;
        return forward_input + right_input;
    }

    private void rotateRigidBodyToVector(Vector3 vector,float multiplier)
    {
        if (vector == Vector3.zero) return;
        Vector3 force_vector = new Vector3(vector.x, 0.0f, vector.z);
        Quaternion targetRotation = Quaternion.LookRotation(force_vector);
        Quaternion lerpedRotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, boat_rotation_speed * multiplier);
        rigidbody.MoveRotation(lerpedRotation);
    }

    private void enter_state(STATES state)
    {
        enter_state(state, new List<string>());
    }
    private void enter_state(STATES new_state, List<string> msg)
    {
        exit_state(state);
        switch(new_state)
        {
        case STATES.Idle:
            break;
        case STATES.Moving:
            break;
        case STATES.Charge:
                print("LEL");
                // Makes the boat tip up slightly when charging
                rigidbody.AddForceAtPosition(new Vector3(0.0f, 5.0f, 0.0f), transform.position + transform.forward * 1.0f, ForceMode.Impulse);
                Camera.main.GetComponent<PlayerCamera>().set_to_fov(70.0f);
            break;
        }
        state = new_state;
    }

    private void exit_state(STATES old_state)
    {
        switch (old_state)
        {
            case STATES.Idle:
                break;
            case STATES.Moving:
                break;
            case STATES.Charge:
               
                Camera.main.GetComponent<PlayerCamera>().set_to_fov(60.0f);
                break;
        }
    }

}
