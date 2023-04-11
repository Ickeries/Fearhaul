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
        switchWeapon(1);
        weaponInput = GetComponent<PlayerInput>();
        readyToShoot = true;
        Physics.IgnoreLayerCollision(6, 4, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //pivot.transform.eulerAngles = new Vector3(0.0f, Camera.main.transform.rotation.y, 0.0f);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150.0f, collideWith))
        { 
            Ray ray2 = new Ray(pivot.transform.position, (hit.point - pivot.transform.position).normalized);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, 150.0f, collideWith))
            {
                Debug.DrawLine(pivot.transform.position, hit2.point);
                targetPoint = Vector3.Lerp(targetPoint, hit2.point, 10.0f * Time.deltaTime);
            }
        }
        else
        {
            targetPoint = Vector3.Lerp(targetPoint, ray.GetPoint(150.0f), 10.0f * Time.deltaTime);

        }
        pivot.transform.LookAt(targetPoint);
        if (weaponInput.actions["Fire"].ReadValue<float>() == 1)
        {
            if(currentWeapon != null)
            {
                currentWeapon.GetComponent<Weapon>().Shoot();
            }
        }
    }

    private void OnWeapon1()
    {
        switchWeapon(0);
    }

    private void OnWeapon2()
    {
        switchWeapon(1);
    }

    private void OnWeapon3()
    {
        switchWeapon(2);
    }

    List<TargetData> getValidTargets()
    {
        var validTargets = new List<TargetData>();
        // Return null if there are no targets
        if (targets.Count == 0)
            return validTargets;

        foreach (GameObject target in targets)
        {
            Vector2 camera_forward_position = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
            Vector2 target_local_position = new Vector2(target.transform.position.x - this.transform.position.x, target.transform.position.z - this.transform.position.z);
            float targetAngle = Vector2.Angle(camera_forward_position, target_local_position);
            // Add target only if within max aim angle.
            if (targetAngle <= maxAimAngle) 
            { 
                TargetData data = new TargetData();
                data.gameObject = target;
                data.angle = targetAngle;
                //data.distance = HandleUtility.DistancePointLine(target.transform.position, Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 100.0f);
                validTargets.Add(data); 
            }
        }
        return validTargets;
    }

    private void switchWeapon(int idx)
    {
        // Check if weapon list idx exists
        if (weaponList.Length < idx)
            return;

        // Check if there is a weapon there
        if (weaponList[idx] == null)
            return;

        currentWeapon = weaponList[idx];
        for(int i = 0; i < weaponList.Length; i++)
        {
            if (i == idx)
            {
                weaponList[i].SetActive(true);
            }
            else
            {
                weaponList[i].SetActive(false);
            }
        }
    }

    private int sortByDistance(TargetData a, TargetData b)
    {
        if (a.distance > b.distance)
            return 1;
        if (a.distance < b.distance)
            return -1;
        return 0;

    }


	private RaycastHit getCameraRaycastHit()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay( new Vector3(0.5f, 0.5f, 0.0f) );
		Physics.Raycast(ray, out hit);
		return hit;
	}

}
