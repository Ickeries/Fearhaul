using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // References
    public Slider slider;
    public List<GameObject> Loot = new List<GameObject>();

    public int maxHealth = 100;
    private int currentHealth = 100;
    private int lerpedHealth = 100;

    private int maxStagger = 100;
    private int currentStagger = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentStagger = maxStagger;
    }

    // Update is called once per frame
    void Update()
    {
        lerpedHealth = (int)Mathf.Lerp(lerpedHealth, currentHealth, 2.5f * Time.deltaTime);
        if (slider != null)
        {
            slider.value = (float)lerpedHealth / (float)maxHealth;
        }
    }

    public void addHealth(float health)
    {
        currentHealth = (int)Mathf.Clamp(currentHealth + health, 0, maxHealth);
        if (health < 0)
        {

        }
    }

    public bool isDead()
    {
        return lerpedHealth <= 0.0;
    }


    public void addStagger(int amount)
    {
        currentStagger += amount;
    }

    public bool isStaggered()
    {
        return currentStagger > maxStagger;
    }

    public void spawnRandomLoot(int amount, Vector3 atPosition)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject LootInstance = Instantiate(Loot[Random.Range(0, Loot.Count)], this.transform.position, Quaternion.identity);
            // Makes the loot shoot upwards for some oomph.
            if (LootInstance.GetComponent<Rigidbody>() != null)
            {
                var launchDirection = new Vector3(Random.Range(-1.0f, 1.0f) * 10.0f, Random.Range(4.0f, 8.0f) * 10.0f, Random.Range(-1.0f, 1.0f) * 10.0f);
                LootInstance.GetComponent<Rigidbody>().AddForce(launchDirection, ForceMode.Impulse);
            }
        }

    }
}
