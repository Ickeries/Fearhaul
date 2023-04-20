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
    public Transform aim;

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
        // Set position to follow
        if(follow != null)
        {
            pivot.position = Vector3.Lerp(pivot.position, follow.position, 5.0f * Time.deltaTime);
        }
        // Look at lock on target
        if (lockOnTarget != null) 
        { 
            pivot.transform.LookAt(lockOnTarget.transform.position);
            rotationX = pivot.transform.rotation.eulerAngles.x;
            rotationY = pivot.transform.rotation.eulerAngles.y;
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        }

        if (lockOnTarget == null)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(0f, 0f, -15.0f), 5.0f * Time.deltaTime);
        }
        else
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(0f, 0f, -15.0f), 55.0f * Time.deltaTime);
        }

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


    public void OnLook(InputValue movementValue)
    {
        rotationX -= movementValue.Get<Vector2>().y * rotationSpeed * Time.deltaTime;
        rotationY += movementValue.Get<Vector2>().x * rotationSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -45.0f, 25.0f);
    }


    public void set_to_fov(float new_fov)
    {
        to_fov = new_fov;
    }
}
