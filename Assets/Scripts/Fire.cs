using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireLoop;
    public GameObject FireExtinguish;

    public void Extinguish()
    {
        FireLoop.SetActive(false);
        FireExtinguish.SetActive(true);
        Destroy(gameObject, 2);
    }
}
