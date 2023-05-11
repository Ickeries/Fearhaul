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

    void Update()
    {
        coinCounter.text = collectables.coins.ToString();
        health.text = ((int)player.lerpedHealth).ToString();
    }
}
