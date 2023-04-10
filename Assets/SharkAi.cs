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

    private float wanderTime = 2.0f;
    private Vector3 wanderPosition = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update

    private float staggerTimer = 1.0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        buoyancy = GetComponent<Buoyancy>();
    }

    void Update()
    {
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0)
        {
            wanderPosition = this.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized * 32.0f;
            wanderTime = 5.0f;
        }

        if (stats.getStaggerAmount() > 10)
        {
            state = STATES.Staggered;
            staggerTimer = 1.0f;
            stats.setStaggerAmount(0);
            animator.Play("hurt", 0, 0.0f);
        }
    }

    void FixedUpdate()
    {
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
                    this.transform.LookAt(wanderPosition);
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
                        rigidbody.AddForce(direction * 50.0f, ForceMode.Force);
                    }
                    this.transform.LookAt(target.transform.position);
                }
                break;
            case STATES.Staggered:
                staggerTimer -= Time.deltaTime;
                if (staggerTimer < 0.0f && buoyancy.is_underwater() == true)
                {
                    state = STATES.Alert;
                }
                break;
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
