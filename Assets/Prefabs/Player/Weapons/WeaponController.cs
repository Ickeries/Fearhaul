using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public float maxAimAngle = 45.0f;
    List<GameObject> targets = new List<GameObject>();
    List<TargetData> validTargets = new List<TargetData>();
	
	public Transform WeaponBase;
	public Transform WeaponBarrel;
	public Transform ProjectileSpawnPosition;

	private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(6, 4, true);
    }

    // Update is called once per frame
    void Update()
    {
		hit = getCameraRaycastHit();
		if (hit.collider != null)
		{
			Debug.DrawLine(ProjectileSpawnPosition.position, hit.point, Color.white, 0.0f);
		}
		//Debug.DrawRay(ProjectileSpawnPosition.position, (target_position - ProjectileSpawnPosition.position)* 50.0f, Color.white, 0.01f);
		//WeaponBase.eulerAngles = new Vector3(0.0f, Camera.main.transform.eulerAngles.y, 0.0f) ;
		WeaponBarrel.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z) ;
        // Calculates the best auto-target
        validTargets = getValidTargets();
        if (validTargets.Count > 0)
        {
            validTargets.Sort(sortByDistance);
            validTargets[0].gameObject.GetComponent<Targetable>().targeted = 0.25f;
            this.transform.LookAt(validTargets[0].gameObject.transform);
        }
    }

	void OnFire(InputValue fireValue)
	{
		if (hit.collider != null)
		{
			print(hit.collider.name);
		}
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
                data.distance = HandleUtility.DistancePointLine(target.transform.position, Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 100.0f);
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
