using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int health = 100;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurt(int amount)
    {
        health -= amount;
        slider.value = health / 100.0f;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
