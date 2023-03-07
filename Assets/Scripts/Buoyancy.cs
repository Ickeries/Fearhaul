using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Buoyancy : MonoBehaviour
{
    public Transform[] floaters;

    public float underWaterDrag = 3;
    public float underWaterAngularDrag = 1;
    public float airDrag = 0f;
    public float airAngularDrag = 0;
    public float floatingPower = 15f;
    public float waterHeight = 0f;
    Rigidbody rigidbody;
    int floatersUnderwater;
    bool underwater;

    private float movement = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        movement = movementValue.Get<Vector2>().x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - waterHeight;
            if (difference < 0)
            {
                rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(underwater);
                }
            }
        }

        if (underwater && floatersUnderwater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            rigidbody.drag = underWaterDrag;
            rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rigidbody.drag = airDrag;
            rigidbody.angularDrag = airAngularDrag;
        }
    }

    public bool is_underwater()
    {
        return underwater;
    }
}