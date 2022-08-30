using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSceneUtility : MonoBehaviour
{
    public float ReloadTime;

    void Start()
    {
        StartCoroutine(LoadCurrentLevelCoroutine());
    }

    private IEnumerator LoadCurrentLevelCoroutine()
    {
        yield return new WaitForSeconds(ReloadTime);
        PunSceneManager.Instance.LoadCurrentLevel();
    }
}