using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotator : MonoBehaviour
{
    // Before rendering each frame..
    // Update is called once per frame
    public Transform model_transform;

    void Update()
    {

        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        model_transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
