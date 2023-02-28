using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    private Vector3 movement;
    CharacterController character_controller;
    private float speed = 1.0f;
    private float max_speed = 5.0f;
    private Vector3 force;
    private Vector3 gravity;
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
       //this.transform.Rotate(0.0f, 4f * Time.deltaTime, 0.0f, Space.Self);
       //this.transform.Translate(Vector3.right * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
       
    }
}
