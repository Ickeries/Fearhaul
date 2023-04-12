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
    public float maxAimAngle = 45.0f;
    public GameObject projectile_prefab;
    List<GameObject> targets = new List<GameObject>();
    List<TargetData> validTargets = new List<TargetData>();

    public float timeBetweenShooting, spread, reloadTime;
    public int allowButtonHold;
    public bool shooting, readyToShoot, reloading;
    public float shootForce = 10.0f;
    public float upwardForce = 20.0f;
	private RaycastHit hit;
    public AnimationCurve curve;
    private Vector3 targetPoint;
    PlayerInput weaponInput;
    public LayerMask collideWith;
    // Start is called before the first frame update
    void Start()
    {
        weaponInput = GetComponent<PlayerInput>();
        readyToShoot = true;
        Physics.IgnoreLayerCollision(6, 4, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (weaponInput.actions["Fire"].IsPressed())
        {
            currentWeapon.GetComponent<Weapon>().Shoot();
        }
    }





}
