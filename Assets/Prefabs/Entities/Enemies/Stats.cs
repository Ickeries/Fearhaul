using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // References
    public Slider slider;
    public MeshRenderer mesh;
    public Animator animator;
    public GameObject damageText;
    public List<GameObject> Loot = new List<GameObject>();

    public int maxHealth = 100;
    private int currentHealth = 100;
    private int lerpedHealth = 100;

    private int maxStagger = 100;
    private int currentStagger = 0;

    public int lootAmount = 0;
    public bool destroyWhenDead = false;

    private float infoTimer = 0.0f;

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
        
        if (isDead() && destroyWhenDead)
        {
            spawnRandomLoot(lootAmount, this.transform.position);
            Destroy(this.gameObject);
        }
        if (slider != null)
        {
            slider.value = (float)lerpedHealth / (float)maxHealth;
        }

        if (mesh != null)
        {
            Color healthColor = Color.white - (Color.blue + Color.green) * (1 - (float)lerpedHealth / (float)maxHealth);
            mesh.material.SetColor("_Color", healthColor);
        }
    }

    public void addHealth(float health)
    {
        currentHealth = (int)Mathf.Clamp(currentHealth + health, 0, maxHealth);
        if (health < 0f)
        {
            if (animator != null)
            {
                animator.Play("hurt", 0, 0.0f);
            }
            //GameObject damageTextInstance = Instantiate(damageText, this.transform.position + new Vector3(0f, 8.0f, 0.0f), Quaternion.identity);
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

    public void push(Vector3 direction)
    {
        this.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
    }


    public void showInfo()
    {

    }
}
