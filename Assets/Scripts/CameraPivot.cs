using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivot : MonoBehaviour
{
    public Transform follow;
    public float rotationSpeed = 25.0f;

    private Vector3 targetRotation;
    float rotationX = 0.0f;
    float rotationY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

 
    void OnLook(InputValue movementValue)
    {
        rotationX -= movementValue.Get<Vector2>().y * rotationSpeed * Time.deltaTime;
        rotationY += movementValue.Get<Vector2>().x * rotationSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -15.0f, 80.0f);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
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
