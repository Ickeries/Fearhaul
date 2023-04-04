using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float timeAlive = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            other.GetComponent<Stats>().hurt(10);
        }
    }
}
