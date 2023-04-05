using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
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

    PlayerInput weaponInput;
    // Start is called before the first frame update
    void Start()
    {
        weaponInput = GetComponent<PlayerInput>();
        readyToShoot = true;
        Physics.IgnoreLayerCollision(6, 4, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponInput.actions["Fire"].ReadValue<float>() == 1)
        {
            Shoot();
        }
    }


	void OnFire(InputValue fireValue)
	{
	}


    private void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(75);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    targetPoint = hit.point;
        //}
        //else
        // {
        // }
        Vector3 directionWithoutSpread = targetPoint - this.transform.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0.0f);
        GameObject projectile_instance = Instantiate(projectile_prefab, this.transform.position, Quaternion.identity);
        projectile_instance.transform.forward = directionWithSpread.normalized;
        projectile_instance.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        projectile_instance.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * upwardForce, ForceMode.Impulse);



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
	
    void OnTriggerEnter(Collider collider)
    {
        var targetable = collider.gameObject.GetComponent<Targetable>();
        if (targetable)
        {
            targets.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        targets.Remove(collider.gameObject);
    }

}
