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
    private int staggerAmount = 0;
    private int staggerThreshold = 10;
    private bool dead = false;

    private Rigidbody rigidbody;

    private float hurtTimer = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
        slider.value = health;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        hurtTimer -= Time.deltaTime;
        hurtTimer = Mathf.Clamp(hurtTimer, 0, 1.0f);
    }

    public void hurt(int amount, int stagger=0)
    {
        if (hurtTimer > 0)
        {
            return;
        }

        health -= amount;
        staggerAmount += stagger;
        slider.value = (float)health / (float)max_health;
        //GameObject gameTextInstance = Instantiate(damageText, this.transform.position + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);
        if (health <= 0 && !dead)
        {
            dead = true;
            spawnCoins(5, transform.position);
            Destroy(this.gameObject);
        }
        hurtTimer = 0.25f;
    }

    public void launch(Vector3 launchForce, int stagger)
    {
        staggerAmount += stagger;
        if (staggerAmount <= 10)
        { 
            rigidbody.AddForce(launchForce, ForceMode.Impulse);
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

    public int getStaggerAmount()
    {
        return staggerAmount;
    }

    public void setStaggerAmount(int amount)
    {
        staggerAmount = amount;
    }

    public void addStaggerAmount(int amount)
    {
        staggerAmount += amount;
    }

    public bool isStaggered()
    {
        return staggerAmount > staggerThreshold;
    }
}
