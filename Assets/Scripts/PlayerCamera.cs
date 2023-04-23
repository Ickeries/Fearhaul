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
    public Transform followTransform;

    private GameObject lockOnTarget = null;

    public float aiming = 0.0f;

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
        if (followTransform != null)
        {
            pivot.transform.position = followTransform.position;
        }
        pivot.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
    }

    public void lockOnTo(GameObject lockOnObject)
    {
        lockOnTarget = lockOnObject;
    }

    public void resetLockOn()
    {
        lockOnTarget = null;
    }


    public void addZoom(float value)
    {
        Vector3 p = this.transform.localPosition;
        this.transform.localPosition = new Vector3(p.x, p.y, p.z + value);
    }


    public void OnLook(Vector2 movementValue)
    {
        rotationX -= movementValue.y * rotationSpeed * Time.deltaTime;
        rotationY += movementValue.x * rotationSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -45.0f, 25.0f);
    }


    public void set_to_fov(float new_fov)
    {
        to_fov = new_fov;
    }
}
