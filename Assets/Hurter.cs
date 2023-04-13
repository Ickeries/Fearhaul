using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurter : MonoBehaviour
{

    public LayerMask collideWith;
    public int damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if ( (collideWith.value & 1 << other.gameObject.layer) != 0)
        {
            if (other.gameObject.GetComponent<Stats>() != null)
            {
                other.gameObject.GetComponent<Stats>().addHealth(-damage);
            }
        }
    }



}
