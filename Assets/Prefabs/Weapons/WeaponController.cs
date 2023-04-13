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


    public void pickupWeapon(GameObject weapon)
    {
        if (weapon.GetComponent<Weapon>() != null)
        {
            GameObject weapon_instantiated = Instantiate(weapon, this.transform.position, Quaternion.identity);
            weapon_instantiated.transform.parent = this.transform;
            if (currentWeapon != null)
            {
                dropWeapon(currentWeapon);
            }
            currentWeapon = weapon_instantiated;
        }
    }

    public void dropWeapon(GameObject weapon)
    {
        GameObject weaponPickupPrefab = Instantiate(weaponPickupPrefab, this.transform.position, Quaternion.identity);
        weaponPickupPrefab.weapon = PrefabUtility.GetPrefabInstanceHandle(weapon);
        Destroy(weapon);
        currentWeapon = null;
    }

}
