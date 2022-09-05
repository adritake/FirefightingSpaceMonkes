using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCredits : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            PunSceneManager.Instance.LoadMenuScene();
        }
    }
}
