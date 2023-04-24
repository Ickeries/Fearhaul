using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Rigidbody rigidbody;

    public int attackPower = 10;
    public int staggerPower = 10;
    public float timeAlive = 0.5f;
    public float gravityMultiplier = 1.0f;
    public GameObject explosion_prefab;
    public LayerMask collideLayers;
    // Start is called before the first frame update

    void Awake()
    {
        Physics.IgnoreLayerCollision(11, 8);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive -= Time.deltaTime;
        if (timeAlive <= 0.0f)
        {
            Destroy(this.gameObject);
            
        }

        if (this.transform.position.y < 0.0f)
        {
            if (explosion_prefab != null)
            {
                GameObject explosion_instance = Instantiate(explosion_prefab, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            other.GetComponent<Stats>().addHealth(-attackPower);
            other.GetComponent<Stats>().addStagger(10);
        }

        // Check if other is in collideLayers
        if ( (collideLayers.value & 1 << other.gameObject.layer) > 0)
        {
            if (explosion_prefab != null)
            {
                GameObject explosion_instance = Instantiate(explosion_prefab, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
