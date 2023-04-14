using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{

    public GameObject pivot;
    public Transform projectileSpawnTransform;
    public GameObject currentWeapon;
    public GameObject[] weaponList;
    public GameObject weaponPickupPrefab;

    public float maxAimAngle = 45.0f;
    public GameObject projectile_prefab;
    List<GameObject> targets = new List<GameObject>();
    public float timeBetweenShooting, spread, reloadTime;
    public int allowButtonHold;
    public bool shooting, readyToShoot, reloading;
    public float shootForce = 10.0f;
    public float upwardForce = 20.0f;
	private RaycastHit hit;
    public AnimationCurve curve;
    private Vector3 targetPoint;
    public LayerMask collideWith;

    [HideInInspector]
    public bool input_firing = false;

    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        Physics.IgnoreLayerCollision(6, 4, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWeapon != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            Vector3 targetPoint = ray.GetPoint(150.0f);
            if (Physics.Raycast(ray, out hit, 150.0f, collideWith))
            {
               targetPoint = hit.point;
            }

            currentWeapon.GetComponent<Weapon>().Look(targetPoint);
            
            if (input_firing)
            {
                currentWeapon.GetComponent<Weapon>().Shoot();
            }
        }
    }


    public void pickupWeapon(WeaponPickup weaponPickup)
    {
        if (currentWeapon != null)
        {
            dropWeapon(currentWeapon);
        }
        if (weaponPickup.existing_weapon != null)
        {
            weaponPickup.existing_weapon.transform.SetParent(this.transform);
            weaponPickup.existing_weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
            currentWeapon = weaponPickup.existing_weapon;
            weaponPickup.existing_weapon.SetActive(true);
        }
        else
        {
            GameObject weaponInstantiated = Instantiate(weaponPickup.weapon, this.transform.position, Quaternion.identity);
            weaponInstantiated.transform.parent = this.transform;
            
            currentWeapon = weaponInstantiated;
        }
        Destroy(weaponPickup.gameObject);
    }

    public void dropWeapon(GameObject weapon)
    {

        GameObject weaponPickupInstance = Instantiate(weaponPickupPrefab, this.transform.position, Quaternion.identity);
        weaponPickupInstance.GetComponent<WeaponPickup>().existing_weapon = weapon;
        weaponPickupInstance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 15.0f, 0f), ForceMode.Impulse);
        weapon.transform.SetParent(weaponPickupInstance.transform);
        weapon.SetActive(false);
        currentWeapon = null;
    }

}
