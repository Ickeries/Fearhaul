using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    public PlayerCamera camera;
    public BoatController playerBoat;
    public WeaponController weapons;

    public LayerMask focusLayer;
    private GameObject focusedObject;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 200.0f, focusLayer);
        focusedObject = null;
        if (hit.collider != null)
        {
            focusedObject = hit.collider.gameObject;
        }

        if (focusedObject != null)
        {
            switch (focusedObject.tag)
            {
                case "Enemy":
                    focusedObject.GetComponent<Stats>().showInfo();
                    break;
            }
        }

    }

    void OnMove(InputValue movementValue)
    {
        playerBoat.OnMove(movementValue);
    }

    void OnJump()
    {
        playerBoat.Jump();
    }

    void OnLook(InputValue lookValue)
    {
    }

    void OnZoomIn()
    {
        camera.addZoom(-1.0f);
    }

    void OnZoomOut()
    {
        camera.addZoom(1.0f);
    }

    void OnFire(InputValue chargeValue)
    {
        weapons.input_firing = Mathf.Approximately(Mathf.Min(chargeValue.Get<float>(), 1), 1);
    }

    void OnCharge(InputValue chargeValue)
    {
        playerBoat.input_charging = chargeValue.Get<float>();
    }

    void OnInteract()
    {
        if (focusedObject == null) 
        {
            return;
        }


        switch(focusedObject.tag)
        {
            case "WeaponPickup":
                weapons.pickupWeapon(focusedObject.GetComponent<WeaponPickup>());
                break;
        }

        
    }

    void OnAim(InputValue lockOnValue)
    {
        camera.aiming = lockOnValue.Get<float>();
    }

    void OnLockOn(InputValue lockOnValue)
    {
        print(lockOnValue.Get<float>());
        // Pressed
        if(lockOnValue.Get<float>() == 1.0f)
        {
            if (focusedObject == null)
            {
                return;
            }
            switch (focusedObject.tag)
            {
                case "Enemy":
                    camera.lockOnTo(focusedObject.gameObject);
                    break;
            }
        }
        // Unpressed
        else
        {
            camera.resetLockOn();
        }

    }

}
