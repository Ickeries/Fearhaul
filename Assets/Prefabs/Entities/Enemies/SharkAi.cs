using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAi : MonoBehaviour
{
    private enum STATES { Wander, Alert, Staggered }
    private STATES state = STATES.Wander;

    private Rigidbody rigidbody;
    private Stats stats;
    private Buoyancy buoyancy;
    private Animator animator;


    GameObject target = null;

    // Wander
    private float wanderTime = 2.0f;
    private Vector3 wanderPosition = new Vector3(0.0f, 0.0f, 0.0f);


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        buoyancy = GetComponent<Buoyancy>();
    }

    void Update()
    {
        // Every 5 seconds, the entity finds a close position to wander to. IF IN WANDERING mode.
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0)
        {
            wanderPosition = this.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized * 32.0f;
            wanderTime = 5.0f;
        }
        // Check if entity is dead.
        if (stats.isDead())
        {
            stats.spawnRandomLoot(5, this.transform.position);
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        // Gravity
        rigidbody.AddForce(new Vector3(0.0f, -32.0f, 0.0f), ForceMode.Acceleration);

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        switch (state)
        {
            case STATES.Wander:
                float dist = (wanderPosition - this.transform.position).sqrMagnitude;
                if (dist > 8.0f * 8.0f)
                {
                    direction = (wanderPosition - this.transform.position).normalized;
                    if (buoyancy.is_underwater())
                    {
                        rigidbody.AddForce(direction * 50.0f, ForceMode.Force);
                    }
                }


                if (target != null)
                {
                    state = STATES.Alert;
                }
                break;
            case STATES.Alert:
                if (target != null)
                {
                    direction = (target.transform.position - this.transform.position).normalized;
                    if (buoyancy.is_underwater())
                    {
                        rigidbody.AddForce(direction * 80.0f, ForceMode.Force);
                    }
                }
                break;
            case STATES.Staggered:
                break;
        }

        if (direction.sqrMagnitude != 0.0f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, 2.5f * Time.deltaTime);
        }


    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
        }
    }
}
