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
    private GameObject focused_object;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, focusLayer);
        if (hit.collider != null)
        {
            focused_object = hit.collider.gameObject;
        }else
        {
            focused_object = null;
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
        if (focused_object != null && focused_object.tag == "WeaponPickup")
        {
            GameObject weapon = focused_object.GetComponent<WeaponPickup>().getWeapon();
            weapons.pickupWeapon(weapon);
            Destroy(focused_object.gameObject);
        }
    }

}
