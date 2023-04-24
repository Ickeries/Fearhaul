using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerController : MonoBehaviour
{

    public float boatSpeed = 10f;
    public float boatRotationSpeed = 15f;
    public float boatChargeSpeedMultiplier = 1.2f;

    // State Machine
    private PlayerState state;
    private Dictionary<string, PlayerState> stateDictionary = new Dictionary<string, PlayerState>();
   
    public LayerMask aimLayerMask;
    // Accessors
    public Weapon currentWeapon;
    public Transform weaponsTransform;
    public Transform reticleTransform;
    public Transform mainCameraPosition;
    [HideInInspector]
    private Rigidbody rigidbody; 
    public PlayerInput playerInput;
    public Buoyancy buoyancy;
    Transform targetTransform = null;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInput = this.GetComponent<PlayerInput>();
        buoyancy = GetComponent<Buoyancy>();
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

        Ray ray = new Ray(mainCameraPosition.position, Camera.main.transform.forward);
        this.playerInput.actions["Look"].ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, aimLayerMask))
        {
            Ray ray2 = new Ray(currentWeapon.getProjectileSpawnPosition(), (hit.point - currentWeapon.getProjectileSpawnPosition()).normalized);
            RaycastHit hit2;
            if(Physics.Raycast(ray2, out hit2, 100, aimLayerMask))
            { 
                reticleTransform.position = Vector3.Lerp(reticleTransform.position, hit2.point, 5.0f * Time.deltaTime);
                currentWeapon.transform.forward = Vector3.Lerp(currentWeapon.transform.forward, (hit2.point - currentWeapon.getProjectileSpawnPosition()).normalized, 5.0f * Time.deltaTime);
                targetTransform = hit.collider.transform;
            }
        }
        else
        {
            currentWeapon.transform.forward = Vector3.Lerp(currentWeapon.transform.forward, (ray.GetPoint(50.0f) - currentWeapon.getProjectileSpawnPosition()).normalized, 5.0f * Time.deltaTime);
            reticleTransform.position = Vector3.Lerp(reticleTransform.position, ray.GetPoint(50.0f), 5.0f * Time.deltaTime);
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

        if (controller.playerInput.actions["Fire"].ReadValue<float>() == 1.0)
        {
            controller.currentWeapon.Shoot();
            controller.getRigidbody().AddForce(-controller.currentWeapon.transform.forward * controller.currentWeapon.recoilStrength, ForceMode.Impulse);
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
        if (movementInput.y != 0.0f && controller.buoyancy.is_underwater()==true)
        {
            controller.getRigidbody().AddForce(controller.transform.forward * movementInput.y * controller.boatSpeed, ForceMode.Force);
        }


        if (controller.playerInput.actions["Fire"].ReadValue<float>() == 1.0)
        {
            controller.currentWeapon.Shoot();
        }
    }
    public void ExitState()
    {

    }
}