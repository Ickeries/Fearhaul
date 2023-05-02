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
    public PlayerController player;


    private GameObject lockOnTarget = null;
    private float toZoom = 90.0f;
    public float aiming = 0.0f;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.GetComponent<Camera>().fieldOfView, to_fov, 5.0f * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (player.getTarget() != null)
            {
                Vector3 toPosition = player.transform.position + (player.getTarget().transform.position - player.transform.position) * 0.5f;
                pivot.transform.position = Vector3.Lerp(pivot.transform.position, toPosition, 25.0f * Time.deltaTime);
                //Vector3 flattenedVector = new Vector3(player.getTarget().transform.position.x, 0.0f, player.getTarget().transform.position.z);
                //Vector3 direction = (flattenedVector - pivot.transform.position).normalized;
                //Vector3 direction2 = (flattenedVector - transform.position).normalized;
                //pivot.transform.forward = Vector3.Lerp(pivot.transform.forward, direction, 5.0f * Time.deltaTime);
                //transform.forward = Vector3.Lerp(transform.forward, direction2, 5.0f * Time.deltaTime);
                float dist = Vector3.Distance(toPosition, player.transform.position);
                dist = Mathf.Clamp(dist, 90.0f, 500.0f);
                toZoom = dist;
                
            }
            else
            {
                pivot.transform.position = Vector3.Lerp(pivot.transform.position, player.transform.position, 25.0f * Time.deltaTime);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 5.0f * Time.deltaTime);
            }
        }
        Vector3 p = this.transform.localPosition;
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(p.x, p.y, -toZoom), 2.5f * Time.deltaTime);
    }

    public void lockOnTo(GameObject lockOnObject)
    {
        lockOnTarget = lockOnObject;
    }

    public void resetLockOn()
    {
        lockOnTarget = null;
    }

    public void setZoom(float value)
    {
        toZoom = value;
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


    public void setFov(float new_fov)
    {
        to_fov = new_fov;
    }
}
