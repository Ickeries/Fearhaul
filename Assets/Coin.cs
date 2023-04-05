using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    Rigidbody rigidbody;
    // Start is called before the first frame update

    //for the coin magnet
    public float CoinSpeed;
    public Transform player;
    private bool ReadyToMove = false;
    public float coinMagnetSpeed;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // for the coin magnet
        if (ReadyToMove == true)
        {
            rigidbody.AddForce((player.transform.position - this.transform.position).normalized * coinMagnetSpeed , ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Magnet")
        {
            ReadyToMove = true;
            player = collision.transform;
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(new Vector3(0.0f, -32.0f, 0.0f), ForceMode.Acceleration);
    }
}