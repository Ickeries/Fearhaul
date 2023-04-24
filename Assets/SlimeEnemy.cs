using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public Animator animator;
    public Buoyancy buoyancy;
    public Rigidbody rigidbody;
    public Stats stats;
    private SlimeState state;
    private Dictionary<string, SlimeState> stateDictionary = new Dictionary<string, SlimeState>();
    public float jumpStrength = 5.0f;

    public GameObject target;

    public float staggerTimer = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        buoyancy = GetComponent<Buoyancy>();
        rigidbody = GetComponent<Rigidbody>();
        stateDictionary.Add("idle", new SlimeIdleState(this));
        stateDictionary.Add("move", new SlimeMoveState(this));
        stateDictionary.Add("air", new SlimeAirState(this));
        stateDictionary.Add("hurt", new SlimeHurtState(this));
        ChangeState("idle");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state != null)
        {
            state.FixedUpdateState();
        }

    }

    public void ChangeState(string stateName)
    {
        if (state != null)
        {
            state.ExitState();
        }
        state = getState(stateName);
        state.EnterState();
    }

    public SlimeState getState(string stateName)
    {
        return stateDictionary[stateName];
    }


    public void jump()
    {
        this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, jumpStrength, 0.0f), ForceMode.Impulse);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
        }
    }

}


public interface SlimeState
{
    public void EnterState();
    public void UpdateState();
    public void FixedUpdateState();
    public void ExitState();
}



public class SlimeIdleState : SlimeState
{
    SlimeEnemy controller;

    public SlimeIdleState(SlimeEnemy _controller)
    {
        controller = _controller;
    }

    public void EnterState()
    {
        controller.animator.Play("hop", 0, 0.0f);
    }
    public void UpdateState()
    {
    }
    public void FixedUpdateState()
    {
        if (controller.stats.isStaggered() == true)
        {
            controller.ChangeState("hurt");
        }
        else if (controller.buoyancy.is_underwater() == false)
        {
            controller.ChangeState("air");
        }
    }
    public void ExitState()
    {

    }
}

public class SlimeMoveState : SlimeState
{
    SlimeEnemy controller;

    public SlimeMoveState(SlimeEnemy _controller)
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

public class SlimeAirState : SlimeState
{
    SlimeEnemy controller;

    public SlimeAirState(SlimeEnemy _controller)
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
        if (controller.stats.isStaggered() == true)
        {
            controller.ChangeState("hurt");
        }
        else if (controller.buoyancy.is_underwater() == true)
        {
            controller.ChangeState("idle");
        }

        if (controller.target != null)
        {
            Vector3 direction = (controller.target.transform.position - controller.transform.position).normalized;
            controller.rigidbody.AddForce(direction * 20.0f, ForceMode.Force);
            controller.transform.forward = Vector3.Lerp(controller.transform.forward, direction, 2.5f * Time.deltaTime);
        }

    }
    public void ExitState()
    {

    }
}


public class SlimeHurtState : SlimeState
{
    SlimeEnemy controller;

    public SlimeHurtState(SlimeEnemy _controller)
    {
        controller = _controller;
    }

    public void EnterState()
    {
        controller.staggerTimer = 2.0f;
        controller.animator.Play("hurt", 0, 0.0f);
    }
    public void UpdateState()
    {

    }
    public void FixedUpdateState()
    {
        controller.staggerTimer -= Time.deltaTime;
        if (controller.staggerTimer <= 0.0f)
        {
            controller.ChangeState("idle");
        }
    }
    public void ExitState()
    {
        controller.stats.resetStagger();
    }
}

