using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerController : MonoBehaviour
{

    public float boatSpeed = 10f;
    public float boatRotationSpeed = 15f;
    public float boatChargeSpeedMultiplier = 1.2f;

    public GameObject Box;

    // State Machine
    private PlayerState state;
    private Dictionary<string, PlayerState> stateDictionary = new Dictionary<string, PlayerState>();

    public LayerMask aimLayerMask;
    // Accessors
    public GameObject reticle;
    public Weapon currentWeapon;

    [HideInInspector]
    private Rigidbody rigidbody;
    public PlayerInput playerInput;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInput = this.GetComponent<PlayerInput>();

        stateDictionary.Add("idle", new IdleState(this));
        stateDictionary.Add("move", new MoveState(this));
        state = stateDictionary["idle"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(state != null)
        {
            state.FixedUpdateState();
        }
        rigidbody.AddForce(new Vector3(0f, -9.8f, 0f), ForceMode.Acceleration);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        this.playerInput.actions["Look"].ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300, aimLayerMask))
        {
            currentWeapon.gameObject.transform.LookAt(hit.point);
            reticle.transform.position = Vector3.Lerp(reticle.transform.position, hit.point, 3.0f * Time.deltaTime);
            reticle.GetComponent<MeshRenderer>().material.color = Color.red;
            //camera.transform.LookAt(hit.point);
        }
        else
        {
            currentWeapon.gameObject.transform.LookAt(ray.GetPoint(150.0f));
            reticle.transform.position = Vector3.Lerp(reticle.transform.position, ray.GetPoint(50.0f), 3.0f * Time.deltaTime);
            reticle.GetComponent<MeshRenderer>().material.color = Color.white;
            //camera.transform.LookAt(ray.GetPoint(100.0f));
        }

    }

    public void ChangeState(string stateName)
    {
        state.ExitState();
        state = getState(stateName);
        state.EnterState();
    }

    public PlayerState getState(string stateName)
    {
        return stateDictionary[stateName];
    }

    public Rigidbody getRigidbody() 
    { 
        return rigidbody; 
    }


    public Vector3 getMoveInputVector()
    {
        return this.playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public Vector3 getMovementVector()
    {
        Vector2 moveInput = this.playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 forward = Camera.main.transform.forward * moveInput.y;
        Vector3 right = Camera.main.transform.right * moveInput.x;
        return forward + right;
    }

    void OnLook(InputValue lookValue)
    {
        Camera.main.GetComponent<PlayerCamera>().OnLook(lookValue.Get<Vector2>());
    }

    void OnFire()
    {
        currentWeapon.Shoot();
    }

}


public interface PlayerState
{
    public void EnterState();
    public void UpdateState();
    public void FixedUpdateState();
    public void ExitState();
}



public class IdleState: PlayerState
{
    PlayerController controller;

    public IdleState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void EnterState()
    {

    }
    public void UpdateState()
    {
    }
    public void FixedUpdateState()
    {
        Vector3 movementVector = controller.getMovementVector();
        if (movementVector.sqrMagnitude != 0.0f)
        {
            controller.ChangeState("move");
        }
    }
    public void ExitState()
    {

    }
}

public class MoveState : PlayerState
{
    PlayerController controller;

    public MoveState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void EnterState()
    {

    }
    public void UpdateState()
    {

    }
    public void FixedUpdateState()
    {
        Vector3 movementInput = controller.getMoveInputVector();
        Vector3 movementVector = controller.getMovementVector();
        if (movementVector.sqrMagnitude == 0.0f)
        {
            controller.ChangeState("idle");
        }

        if (movementInput.x != 0.0f)
        {
            controller.transform.Rotate(new Vector3(0.0f, 45.0f * movementInput.x * Time.deltaTime, 0.0f));
        }
        if (movementInput.y != 0.0f)
        {
            controller.getRigidbody().AddForce(controller.transform.forward * movementInput.y * controller.boatSpeed, ForceMode.Force);
        }
    }
    public void ExitState()
    {

    }
}