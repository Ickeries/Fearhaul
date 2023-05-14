using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunWeapon : Weapon
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
    [SerializeField] private GameObject alternateProjectile;
    [SerializeField] Transform flashes;
    private float flashesTimer;

    [SerializeField] private AudioClip[] fireAudios;


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
        flashes.localScale = Vector3.Lerp(flashes.localScale, Vector3.zero, 10.0f * Time.deltaTime);
    }

    public override int Fire()
    {
        // Fail State
        if (reloading == true || betweenShotsTimers > 0.0f)
        {
            return 1;
        }
        // Success State
        else
        {
            AudioSource.PlayClipAtPoint(fireAudios[0], this.transform.position, 5.0f);
            muzzleFlash();
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Count - 1)], projectileSpawn.position, Quaternion.identity);
            newProjectile.transform.rotation = this.transform.rotation;
            newProjectile.transform.Rotate(new Vector3(0.0f, Random.Range(-spread, spread), 0.0f));
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

    void muzzleFlash()
    {
        int randomInt = Random.Range(0, flashes.childCount);
        for (int i = 0; i < flashes.childCount; i++)
        {
            if (i == randomInt)
            {
                flashes.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                flashes.GetChild(i).gameObject.SetActive(false);
            }
        }
        flashes.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

}
