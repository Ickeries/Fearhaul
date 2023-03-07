using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class shark_test : MonoBehaviour
{

    public GameObject distanceFromCenterObject;
    private TextMeshPro distanceFromCenterText;
    private Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        distanceFromCenterText = distanceFromCenterObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screen_coordinate = Camera.main.WorldToScreenPoint(transform.position);
        distanceFromCenterText.text = Vector2.Distance(screen_coordinate, center).ToString();
    }
}
