using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    [SerializeField] private int damage = 10;

    bool attacking = false;
    float attackingTimer = 0.0f;

    float hurtTimer = .3f;
    [SerializeField] private Collider hurtbox;
    [SerializeField] private ParticleSystem flames;
    [SerializeField] private float fireAmount = 25.0f;


    private List<Stats> targets = new List<Stats>();

    [SerializeField] private LayerMask collideWith;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        attackingTimer = Mathf.Clamp(attackingTimer - Time.deltaTime, -0.1f, 1.0f);
        if (attacking && attackingTimer <= 0.1)
        {
            flames.Stop();
            attacking = false;
        }

        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0.0f)
        {
            hurtTimer = 0.3f;
            if (attackingTimer > 0.0f)
            {
                for(int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        targets[i].addHealth(-damage);
                        targets[i].addFireAccumulation(fireAmount);
                    }
                    else
                    {
                        targets.Remove(targets[i]);
                    }
                }
            }
        }

    }

    public override int Fire()
    {
        if(attacking == false)
        {
            flames.Play();
        }
        attacking = true;
        attackingTimer = 0.5f;
        return 1;
    }


    void OnTriggerEnter(Collider other)
    {
        if ((collideWith.value & 1 << other.gameObject.layer) > 0)
        {
            if (other.GetComponent<Stats>() != null)
            {
                targets.Add(other.GetComponent<Stats>());
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.GetComponent<Stats>()))
        {
            targets.Remove(other.GetComponent<Stats>());
        }
    }
}
