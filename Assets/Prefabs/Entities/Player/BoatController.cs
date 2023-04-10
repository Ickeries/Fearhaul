using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BoatController : MonoBehaviour
{
    private Buoyancy buoyancy;
    private Rigidbody rigidbody;
    private CharacterController cc;
    // Publics
    public  float boatSpeed = 32.0f;
    public float boatChargeSpeed = 48.0f;
    public float boatRotationSpeed = 100.0f;
    public float jumpStrength = 10.0f;
    public TMP_Text currentStateText;

    private Vector3 force;
    private Vector3 movement;

    List<GameObject> allCollisions = new List<GameObject>();

    enum STATES { Idle, Moving, Charge, Aim}
    private STATES state = STATES.Idle;

    private Quaternion toRotation;

    //Input
    PlayerInput playerInput;
    InputAction JumpActions;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 11);
        playerInput = this.GetComponent<PlayerInput>();
        JumpActions = playerInput.actions["Jump"];
        cc = GetComponent<CharacterController>();
    }

    void OnMove(InputValue movementValue)
    {
        movement.x = movementValue.Get<Vector2>().x;
        movement.z = movementValue.Get<Vector2>().y;
    }

    void OnHop()
    {
    }

    void Update()
    {
        allCollisions.RemoveAll(s => s == null);
        foreach(GameObject collision in allCollisions)
        {
            if (!collision.GetComponent<Stats>().isStaggered())
            {
                Vector3 launchForce = new Vector3(Random.Range(-1.0f, 1.0f) * 2.0f * force.x, 0.1f * force.magnitude, Random.Range(-1.0f, 1.0f) * 2.0f * force.z);
                collision.GetComponent<Stats>().launch(launchForce, 10);
                collision.GetComponent<Stats>().hurt(10);
            } 
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveDirection = get_movement_vector();
        if (movement.sqrMagnitude != 0.0f)
        {
            toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }

        // Calculates input direction with respect to camera direction to create a final input
        switch (state)
        { 
        case STATES.Idle:
                if (movement.sqrMagnitude > 0.0f)
                {
                    enter_state(STATES.Moving);
                }
                force = Vector3.Lerp(force, new Vector3(0.0f, 0.0f, 0.0f), 1.0f * Time.deltaTime);
                break;
        case STATES.Moving:
                if (playerInput.actions["Charge"].IsPressed())
                {
                    enter_state(STATES.Charge);
                }
                else if (movement.sqrMagnitude == 0.0f)
                {
                    enter_state(STATES.Idle);
                }
                force = Vector3.Lerp(force, transform.forward * boatSpeed, 5.0f * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, boatRotationSpeed * Time.deltaTime);
                break;
        case STATES.Charge:
                if (movement.sqrMagnitude == 0.0f)
                {
                    enter_state(STATES.Idle);
                }
                else if (!playerInput.actions["Charge"].IsPressed())
                {
                    enter_state(STATES.Moving);
                }
                force = Vector3.Lerp(force, transform.forward * boatChargeSpeed, 5.0f * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, boatRotationSpeed * Time.deltaTime);
                break;
        }
        cc.Move(force * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }


    private Vector3 get_movement_vector()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        Vector3 input = forward * movement.z + right * movement.x;
        return new Vector3(input.x, 0.0f, input.z);
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
            // Makes the boat tip up slightly when charging
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


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            {
                allCollisions.Add(collision.gameObject);
            }
    }

    void OnCollisionExit(Collision collision)
    {
        if(allCollisions.Contains(collision.gameObject))
        { 
            allCollisions.Remove(collision.gameObject);
        }
    }


}
