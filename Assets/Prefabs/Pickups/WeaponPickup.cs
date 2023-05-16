using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon = null;
    public GameObject existingWeapon = null;
    public GameObject canvas;
    private PlayerController player;

    public GameObject getWeapon()
    {
        if (existingWeapon != null)
            return existingWeapon;
        GameObject weaponInstance = Instantiate(weapon, this.transform.position, Quaternion.identity);
        return weaponInstance;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().weaponPickups.Add(this);
            canvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().weaponPickups.Remove(this);
            canvas.SetActive(false);
        }
    }
}
