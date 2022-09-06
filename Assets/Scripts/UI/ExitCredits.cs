using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCredits : MonoBehaviour
{
    public AudioClip menuSound;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenu();
        }
    }

    public void LoadMenu()
    {
        AudioManager.Instance.PlaySound(menuSound);
        AudioManager.Instance.MenuMusic();
        PunSceneManager.Instance.LoadMenuScene();
    }
}
