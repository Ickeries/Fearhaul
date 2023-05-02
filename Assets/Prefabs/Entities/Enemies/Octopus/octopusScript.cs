using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class octopusScript : MonoBehaviour
{
    private enum STATES {Wander, Alert, Staggered, Shoot}
    private STATES state = STATES.Wander;
    private Rigidbody rigidbody;
    private Stats stats; 
    private Buoyancy buoyancy;
    private Animator animator;

    GameObject target = null;

    private float wanderTime = 2.0f;
    private Vector3 wanderPosition = new Vector3(0.0f, 0.0f, 0.0f);
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        buoyancy = GetComponent<Buoyancy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == STATES.Wander)
        {
            // Entity moves to a new position when in wandering mode.
            wanderTime -= Time.deltaTime;
            if (wanderTime <= 0)
            {
                wanderPosition = this.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized *32.0f;
                wanderTime = 1.0f;
            }
            // Check if entity is dead.
            if (stats.isDead())
            {
                stats.spawnRandomLoot(5, this.transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        //Gravity
        rigidbody.AddForce(new Vector3(0.0f, -32.0f, 0.0f), ForceMode.Acceleration);

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        switch(state)
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
                break;
           case STATES.Alert:
            if (target == null)
            {
                state = STATES.Wander;
            }
            break;
            {
                direction = (target.transform.position - this.transform.position).normalized;
                if (buoyancy.is_underwater())
                {
                    rigidbody.AddForce(direction * 50.0f, ForceMode.Force);
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

public class OctShooting : MonoBehaviour
{
    public Transform enemy;
    public float range = 50.0f;
    public float inkImpulse = 20.0f;

    private bool onRange = false;
    public Rigidbody projectile;

    void Start()
    {
        if (onRange)
        {
            Rigidbody ink = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            ink.AddForce(transform.forward*inkImpulse, ForceMode.Impulse);

            Destroy (ink.gameObject, 2);
        }
    }



}