using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageText : MonoBehaviour
{
    private Vector3 force = new Vector3(0.0f, 64.0f, 0.0f);

    private float timer = 0.5f;
    void Start()
    {
        force += new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f)) * 8.0f;
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        force -= new Vector3(0.0f, 2.0f, 0.0f);
        this.transform.position += force * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
