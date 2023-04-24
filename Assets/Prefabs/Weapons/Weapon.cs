using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    public Transform spawnProjectileTransform;
    public GameObject projectile;
    public Transform pivotY;
    public Transform pivotX;


    public TextMeshPro ammoText;


    public float spread = 5.0f;
    public float timeBetweenShots = 0.5f;
    public float fireSpeed = 100.0f;
    public int clipSize = 10;
    public float reloadTime = 3.0f;
    private float reloadTimer = 3.0f;
    private int currentAmmo = 10;
    private float shootTimer = 0.0f;
    public float recoilStrength = 0.0f;
    public Gradient ammoTextGradient;

    private Vector3 lookAtPosition = new Vector3(0f, 0f, 0f);

    private bool reloading = false;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize;
    }
    
    void FixedUpdate()
    {
        Debug.DrawRay(spawnProjectileTransform.position, -this.spawnProjectileTransform.forward * 200.0f);
    }
    
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0.0f)
        {
            shootTimer= 0.0f;
        }

        if (reloading == true)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0.0f)
            {
                reloading = false;
                reloadTimer = reloadTime;
                currentAmmo = clipSize;
            }
        }
        ammoText.text = currentAmmo.ToString() + " / " + clipSize.ToString();
        ammoText.color = ammoTextGradient.Evaluate((float)currentAmmo / (float)clipSize);
    }
    public void Look(Vector3 atPosition)
    {
       lookAtPosition = Vector3.Lerp(lookAtPosition, atPosition, 10.0f * Time.deltaTime);
       pivotY.LookAt(lookAtPosition);
       //pivotX.rotation = Quaternion.Lerp(pivotX.rotation, toRotationX, 1.0f * Time.deltaTime);
    }

    public Vector3 getProjectileSpawnPosition()
    {
        return spawnProjectileTransform.position;
    }

    // Returns recoil strength. Make use of that whoever calls this.
    public void Shoot()
    {
        if (shootTimer != 0.0f || reloading == true)
        {
            return;
        }

        if (currentAmmo > 0)
        {
            animator.Play("fire", 0, 0.0f);
            shootTimer = timeBetweenShots;
            float xSpread = Random.Range(-1.0f, 1.0f);
            float ySpread = Random.Range(-1.0f, 1.0f);
            Vector3 spreadVector = new Vector3(xSpread, ySpread, 0.0f).normalized * spread;
            Quaternion rotation = Quaternion.Euler(spreadVector) * pivotX.rotation;
            GameObject new_projectile = Instantiate(projectile, spawnProjectileTransform.position, rotation);
            new_projectile.GetComponent<Rigidbody>().AddForce(new_projectile.transform.forward * fireSpeed, ForceMode.Impulse);
            currentAmmo--;
            if (currentAmmo <= 0)
            {
                reloading = true;
            }

        }
    }
}
