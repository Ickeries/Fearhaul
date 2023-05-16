using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    [SerializeField] Collectables collectables;
    [SerializeField] PlayerController player;
    [SerializeField] private TextMeshProUGUI coinCounter;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private TextMeshProUGUI gas;
    [SerializeField] private TextMeshProUGUI weapon;
    void Update()
    {
        coinCounter.text = collectables.coins.ToString();
        health.text = ((int)player.lerpedHealth).ToString();
        ammo.text = ((int)player.getCurrentWeapon().getAmmo()).ToString();
        gas.text = ((int)player.getGas()).ToString();

        if (player.getCurrentWeapon() != null)
        {
            weapon.text = "Lv." + player.getCurrentWeapon().getLevel().ToString() + " " + player.getCurrentWeapon().getName();
        }
        else
        {
            weapon.text = "No weapon";
        }
    }



}
