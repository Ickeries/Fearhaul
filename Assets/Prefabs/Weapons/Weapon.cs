using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    // Components and Children
    private Animator animator;
    public Transform projectileSpawn;

    // Attributes
    [SerializeField] private float spread = 1.0f; // value in degrees
    [SerializeField] private float timeBetweenShots = 0.5f;
    private float betweenShotsTimers = 0.0f;
    [SerializeField] private float shootSpeedMultiplier = 1.1f;
    [SerializeField] private float recoilStrength = 0.1f;

    // Reloading
    [SerializeField] private float timeToReload = 1.0f;
    private float reloadTimer = 0.0f;
    private bool reloading = false;

    // Ammo
    [SerializeField] private int maxAmmo = 999;
    [SerializeField] private int clipSize = 24;
    private int currentAmmo = 0;

    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();

    void Start()
    {
        currentAmmo = clipSize;
    }

    void FixedUpdate()
    {
        if (reloading == true)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0.0f)
            {
                reloading = false;
                currentAmmo = clipSize;
            }
        }
        betweenShotsTimers = Mathf.Clamp(betweenShotsTimers - Time.deltaTime, -1.0f, timeBetweenShots);
    }

    public int Fire()
    {
        // Fail State
        if (reloading == true || betweenShotsTimers > 0.0f)
        {
            return 1;
        }
        // Success State
        else
        {
            Vector3 spreadDirection = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 1.0f);
            Vector3 direction = projectileSpawn.TransformVector(spreadDirection);
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Count - 1)], projectileSpawn.position, Quaternion.identity);
            
            newProjectile.GetComponent<Projectile>().setDirection(direction);
            currentAmmo -= 1;
            if (currentAmmo <= 0)
            {
                reloading = true;
                reloadTimer = timeToReload;
            }
            betweenShotsTimers = timeBetweenShots;
            return 0;
        }
        return 1;
    }
}
