using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private float to_fov = 60.0f;
    private Vector3 targetRotation;
    float rotationX = 0.0f;
    float rotationY = 0.0f;
    public float rotationSpeed = 25.0f;

    public Transform pivot;
    public Transform follow;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.GetComponent<Camera>().fieldOfView, to_fov, 5.0f * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (follow != null)
        {
            pivot.position = follow.position;
        }
        pivot.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
    }

    public void addZoom(float value)
    {
        Vector3 p = this.transform.localPosition;
        this.transform.localPosition = new Vector3(p.x, p.y, p.z + value);
    }


    public void OnLook(InputValue movementValue)
    {
        rotationX += movementValue.Get<Vector2>().y * rotationSpeed * Time.deltaTime;
        rotationY += movementValue.Get<Vector2>().x * rotationSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -30.0f, 60.0f);
    }


    public void set_to_fov(float new_fov)
    {
        to_fov = new_fov;
    }
}
