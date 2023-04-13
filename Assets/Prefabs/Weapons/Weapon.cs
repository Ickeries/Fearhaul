using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform spawnProjectileTransform;
    public GameObject projectile;
    public Transform pivotY;
    public Transform pivotX;





    public float spread = 5.0f;
    public float timeBetweenShots = 0.5f;
    public float fireSpeed = 100.0f;
    public int clipSize = 10;
    public float reloadTime = 3.0f;
    private int currentAmmo = 10;
    private float shootTimer = 0.0f;

    private Vector3 lookAtPosition = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize;
    }
    
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0.0f)
        {
            shootTimer= 0.0f;
        }
    }
    public void Look(Vector3 atPosition)
    {
       lookAtPosition = Vector3.Lerp(lookAtPosition, atPosition, 2.5f * Time.deltaTime);
       pivotY.LookAt(lookAtPosition);
       //pivotX.rotation = Quaternion.Lerp(pivotX.rotation, toRotationX, 1.0f * Time.deltaTime);
    }

    public void Shoot()
    {
        // 
        if (shootTimer != 0.0f)
        {
            return;
        }
        if (currentAmmo > 0)
        {
            shootTimer = timeBetweenShots;
            float xSpread = Random.Range(-1.0f, 1.0f);
            float ySpread = Random.Range(-1.0f, 1.0f);
            Vector3 spreadVector = new Vector3(xSpread, ySpread, 0.0f).normalized * spread;
            Quaternion rotation = Quaternion.Euler(spreadVector) * pivotX.rotation;
            GameObject new_projectile = Instantiate(projectile, spawnProjectileTransform.position, rotation);
            new_projectile.GetComponent<Rigidbody>().AddForce(new_projectile.transform.forward * fireSpeed, ForceMode.Impulse);
            currentAmmo--;
        }
        else
        {
            currentAmmo = clipSize;
        }
    }
}
