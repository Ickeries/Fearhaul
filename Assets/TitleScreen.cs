using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public string sceneName;

    [SerializeField] private Button startButton;


    private AssetBundle mainBundle;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(Started);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Started()
    {
        SceneManager.LoadScene(sceneName);
    }

}
