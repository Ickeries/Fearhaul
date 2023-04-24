using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
	float x2;
	float y2;
	void Update () {
		float y = Input.GetAxis("Mouse Y") * 10;
		float x = Input.GetAxis("Mouse X") * 10;
		 x2 = Mathf.Lerp(x2, x, Time.deltaTime);
		 y2 = Mathf.Lerp(y2, y, Time.deltaTime);
		transform.rotation = Quaternion.Euler(-y2,x2,0);
	}
}
