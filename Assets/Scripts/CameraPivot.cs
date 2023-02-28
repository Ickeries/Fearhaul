using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivot : MonoBehaviour
{

    private float movement = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue movementValue)
    {
        movement = movementValue.Get<Vector2>().x;
       
    }

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.0f, movement * 50.0f * Time.deltaTime, 0.0f));
    }
}
