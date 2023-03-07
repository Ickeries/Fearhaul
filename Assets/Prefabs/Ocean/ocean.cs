using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ocean : MonoBehaviour
{
    public Transform follow;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(follow.position.x, 0.0f, follow.position.z);
    }
}
