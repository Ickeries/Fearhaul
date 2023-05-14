using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public Animator animator;
    public Buoyancy buoyancy;
    public Rigidbody rigidbody;
    public Stats stats;

    enum States {wander, circle, chase}
    [SerializeField] private States state = States.wander;
    private Vector3 originPosition;

    [SerializeField] private float timeBetweenWander = 5.0f;
    private Vector3 wanderOriginOffset;

    private float wanderTime;

    private Vector3 direction;

    public GameObject target;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        originPosition = this.transform.position;
        wanderTime = timeBetweenWander;
    }

    void Update()
    {
        wanderTime -= Time.deltaTime;
        if (wanderTime < 0f) 
        {
            wanderOriginOffset = new Vector3(Random.Range(-32.0f, 32.0f), 0.0f, Random.Range(-32.0f, 32.0f));
            wanderTime = timeBetweenWander;
        }
    }

    void FixedUpdate()
    {
        switch(state) 
        {
            case States.wander:
                moveTo(originPosition + wanderOriginOffset);
                if (target != null)
                {
                    state = States.chase;
                }
                break;
            case States.circle:
                break;
            case States.chase:
                rigidbody.AddForce((target.transform.position - this.transform.position).normalized * 50.0f, ForceMode.Force);
                break;
        }

        this.transform.forward = Vector3.Lerp(this.transform.forward, direction, 5.0f * Time.deltaTime);
    }


    void moveTo(Vector3 toPosition)
    {

        direction = (toPosition - this.transform.position).normalized;
        direction = new Vector3(direction.x, 0.0f, direction.z);
        rigidbody.AddForce(this.transform.forward * 30.0f, ForceMode.Force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.gameObject;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().hurt(10);
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 10.0f, 0.0f), ForceMode.Impulse);
        }
    }
}
