using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance
    {
        get { return instance ?? (instance = new GameObject("Manager").AddComponent<Manager>()); }
    }
    public void spawnGameObject(GameObject gobj, Transform at_transform, int value)
    {
        GameObject damageTextInstance = Instantiate(gobj, at_transform);
        damageTextInstance.transform.position = at_transform.position;
    }
}
