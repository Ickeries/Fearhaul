using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{



    public float timeAlive = 0.5f;

    public GameObject explosion_prefab;
    // Start is called before the first frame update

    void Awake()
    {
        Physics.IgnoreLayerCollision(11, 8);
    }


    // Update is called once per frame
    void Update()
    {
        timeAlive -= Time.deltaTime;
        if (timeAlive < 0.0f)
        {
            print("Dead");
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



    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            other.GetComponent<Stats>().hurt(10);
        }
        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            if (explosion_prefab != null)
            {
                GameObject explosion_instance = Instantiate(explosion_prefab, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
