using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    [SerializeField] Collectables collectables;
    [SerializeField] private TextMeshProUGUI coinCounter;


    void Update()
    {
        coinCounter.text = collectables.coins.ToString() + "/ 100";
    }
}
