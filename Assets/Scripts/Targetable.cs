using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public GameObject target;
    public float targeted = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targeted > 0.0f)
        {
            target.SetActive(true);
        }
        else
        {
            target.SetActive(false);
        }
        targeted -= Time.deltaTime;
    }
}
