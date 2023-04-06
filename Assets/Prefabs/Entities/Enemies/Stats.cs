using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int max_health = 100;
    int health = 100;
    public Slider slider;
    public GameObject damageText;
    public GameObject coin;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurt(int amount)
    {
        health -= amount;
        slider.value = (float)health / (float)max_health;
        GameObject gameTextInstance = Instantiate(damageText, this.transform.position + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);

        print(health);
        if (health <= 0 && !dead)
        {
            dead = true;
            spawnCoins(5, transform.position);
            Destroy(this.gameObject);
        }
    }

    void spawnCoins(int amountOfCoins, Vector3 at_position)
    {
        for (int i = 0; i < amountOfCoins; i++)
        {
            var random_force = new Vector3(Random.Range(-1.0f, 1.0f) * 10.0f, Random.Range(4.0f, 8.0f) * 10.0f, Random.Range(-1.0f, 1.0f) * 10.0f);
            GameObject coinInstance = Instantiate(coin, this.transform.position, Quaternion.identity);
            // Makes all the coins shoot upwards
            coinInstance.GetComponent<Rigidbody>().AddForce(random_force, ForceMode.Impulse);
            
        }

    }

}
