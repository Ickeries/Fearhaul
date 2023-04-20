using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerController : MonoBehaviour
{

    public float boatSpeed = 50f;
    public float boatRotationSpeed = 15f;
    public float boatChargeSpeedMultiplier = 1.2f;

    // State Machine
    private PlayerState state;
    private IdleState stateIdle;
    private MoveState stateMove;

    // Accessors
    private Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();

        stateIdle = new IdleState(this);
        stateMove = new MoveState(this);
        state = stateIdle;
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
    }

    void ChangeState(PlayerState newState)
    {
        state.ExitState();
        state = newState;
        state.EnterState();
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
        controller.print(controller);
        controller.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 5.0f);
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

    }
    public void ExitState()
    {

    }
}