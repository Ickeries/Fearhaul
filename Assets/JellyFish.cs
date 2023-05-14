using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : MonoBehaviour
{

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float waitTime = 1.0f;
    private float waitTimer = 0.0f;
    private int pathIndex = -1;
    [SerializeField] private Transform path;
    // Start is called before the first frame update
    private Rigidbody rigidbody;

    private List<Vector3> pathPositions = new List<Vector3>();

    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        for (int i = 0; i < path.transform.childCount; i++)
        {
            Vector3 pos = path.transform.GetChild(i).position;
            pathPositions.Add(pos);
            if (pathIndex == -1)
            {
                pathIndex = i;
            }
            else
            {
                if ((pos - this.transform.position).sqrMagnitude < (path.transform.GetChild(pathIndex).position - this.transform.position).sqrMagnitude)
                {
                    pathIndex = i;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are nodes in the path
        if (pathPositions.Count > 0)
        {
            if (pathIndex > pathPositions.Count - 1)
            {
                pathIndex = 0;
            }
            Vector3 nextPathPosition = pathPositions[pathIndex];
            Vector3 direction = nextPathPosition - this.transform.position;
            float distance = new Vector3(direction.x, 0.0f, direction.z).sqrMagnitude;

            float finalSpeed = speed;
            if (distance < speed)
            {
                rigidbody.AddForce(direction, ForceMode.Force);
            }
            else
            {
                rigidbody.AddForce(direction.normalized * finalSpeed, ForceMode.Force);
            }
          

            if (distance < 5.0f)
            {
                pathIndex += 1;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().hurt(10);
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 25.0f, 0.0f), ForceMode.Impulse);
            GetComponent<Stats>().addHealth(-1000);
        }
    }
}
