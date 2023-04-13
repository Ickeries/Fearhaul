using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform pivot;
    public float spinSpeed = 90.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.Rotate(new Vector3(0.0f, spinSpeed * Time.deltaTime, 0.0f));
    }
}
