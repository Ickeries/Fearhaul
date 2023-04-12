using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    public PlayerCamera camera;
    public BoatController playerBoat;
    public WeaponController weapons;

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
        camera.OnLook(lookValue);
    }

    void OnZoomIn()
    {
        camera.addZoom(-1.0f);
    }

    void OnZoomOut()
    {
        camera.addZoom(1.0f);
    }

    void OnFire()
    {
        weapons.OnFire();
    }

    void OnCharge(InputValue chargeValue)
    {
        playerBoat.input_charging = chargeValue.Get<float>();
    }



}
