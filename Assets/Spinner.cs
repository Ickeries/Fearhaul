using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
   void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0.0f, 90.0f * Time.deltaTime, 0.0f));
    }
}
