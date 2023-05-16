using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // References
    public Slider slider;
    public Animator animator;
    public GameObject damageText;
    public List<GameObject> Loot = new List<GameObject>();

    public int maxHealth = 100;
    private int currentHealth = 100;
    private int lerpedHealth = 100;

    public int maxStagger = 100;
    private int currentStagger = 0;

    public int lootAmount = 0;
    public float lootLaunchStrength = 10.0f;
    public bool destroyWhenDead = false;

    private float infoTimer = 0.0f;
    public float Height = 1.0f;
    [SerializeField] private GameObject explosion;
    [SerializeField] private ParticleSystem burningParticles;
    [SerializeField] private float fireAccumulationThreshold = 100f;
    [SerializeField] private float fireDissipationSpeed = 1.0f;
    [SerializeField] private float burningTickRate = 0.5f;
    private float burningTick = 0.0f;
    private float fireAccumulation = 0.0f;
    private bool burning = false;
 

    private Vector3 knockBack;
    [SerializeField] private AudioClip hurtSound;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        lerpedHealth = maxHealth;
        currentStagger = 0;
    }

    // Update is called once per frame
    void Update()
    {
        knockBack = Vector3.Lerp(knockBack, new Vector3(0f, 0f, 0f), 5.0f * Time.deltaTime);
        lerpedHealth = (int)Mathf.Lerp(lerpedHealth, currentHealth, 10.0f * Time.deltaTime);
        
        if (isDead() && destroyWhenDead)
        {
            createExplosion(6.0f);
            spawnRandomLoot(lootAmount, this.transform.position);
            Destroy(this.gameObject);
        }
        if (slider != null)
        {
            slider.value =  (float)lerpedHealth / (float)maxHealth;
        }

        fireAccumulation -= fireDissipationSpeed * Time.deltaTime;
        fireAccumulation = Mathf.Clamp(fireAccumulation, 0, fireAccumulationThreshold + 1.0f);

        if (burning == true)
        {
            burningTick += Time.deltaTime;
            if (burningTick > burningTickRate)
            {
                burningTick = 0.0f;
                addHealth(-5f);
            }

            if (fireAccumulation <= 0.1f)
            {
                burning = false;
                if(burningParticles != null)
                {
                    burningParticles.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            burningTick = 0.0f;
        }

    
    }

    public void addHealth(float health)
    {
        
        currentHealth = (int)Mathf.Clamp(currentHealth + health, 0, maxHealth);
        if (health < 0f)
        {
            if (hurtSound != null)
            {
                AudioSource.PlayClipAtPoint(hurtSound, this.transform.position, 5.0f);
            }
            if(animator != null)
            {
                animator.Play("hurt", 0, 0.0f);
            }

            if (damageText != null)
            {
                GameObject damageTextInstance = Instantiate(damageText, this.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(5.0f, 10.0f), Random.Range(-5.0f, 5.0f)), Quaternion.identity);
                damageTextInstance.GetComponent<DamageText>().setValue((int)health);
            }
        }
    }

    public void addFireAccumulation(float accumulation)
    {
        if (burning == false)
        {
            fireAccumulation += accumulation;
        }
        if (fireAccumulation >= fireAccumulationThreshold && burning == false)
        {
            burning = true;
            if (burningParticles != null)
            {
                burningParticles.gameObject.SetActive(true);
            }
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

    public void resetStagger()
    {
        currentStagger = 0;
    }

    public void spawnRandomLoot(int amount, Vector3 atPosition)
    {
        if (Loot.Count == 0)
            return;

        for (int i = 0; i < amount; i++)
        {
            GameObject LootInstance = Instantiate(Loot[Random.Range(0, Loot.Count)], this.transform.position, Quaternion.identity);
            // Makes the loot shoot upwards for some oomph.
            if (LootInstance.GetComponent<Rigidbody>() != null)
            {
                var launchDirection = new Vector3(Random.Range(-1.0f, 1.0f) * lootLaunchStrength, Random.Range(4.0f, 8.0f) * lootLaunchStrength, Random.Range(-1.0f, 1.0f) * 10.0f);
                LootInstance.GetComponent<Rigidbody>().AddForce(launchDirection, ForceMode.Impulse);
            }
        }

    }

    public void createExplosion(float newScale)
    {
        if (explosion != null)
        {
            GameObject explosionInstance = Instantiate(explosion, this.transform.position, Quaternion.identity);
            explosionInstance.transform.localScale = Vector3.one * newScale;
        }
    }

    public void push(Vector3 direction)
    {
        this.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
    }


    public bool setKnockBack(Vector3 force)
    {
        if (knockBack.sqrMagnitude <= 0.1f)
        {
            knockBack = force;
            GetComponent<Rigidbody>().AddForce(knockBack, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 10.0f, 0.0f), ForceMode.Impulse);
            return true;
        }
        return false;
    }

    public int getHealth()
    {
        return lerpedHealth;
    }

    public void showInfo()
    {

    }
}
