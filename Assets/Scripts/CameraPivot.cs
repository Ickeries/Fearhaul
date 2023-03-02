using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivot : MonoBehaviour
{
    public Transform follow;


    private float movement = 0.0f;
    private Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

 
    void OnLook(InputValue movementValue)
    {
        transform.Rotate(0.0f, movementValue.Get<Vector2>().x * 0.25f, 0.0f, Space.Self);
    }

    void Update()
    {
        transform.position = follow.position;
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
