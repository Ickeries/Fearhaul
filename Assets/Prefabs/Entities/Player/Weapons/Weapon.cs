using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] int Level;
    [SerializeField] protected float damageMultiplier = 1.0f;
    [SerializeField] AnimationCurve damageCurve;

    public virtual int Fire()
    {
        return 1;
    }


    public virtual float getAmmo()
    {
        return 0.0f;
    }

    public virtual void LevelUp()
    {
        Level += 1;
        damageMultiplier += 0.25f;
    }

    public string getName()
    {
        return Name;
    }

    public int getLevel()
    {
        return Level;
    }

}
