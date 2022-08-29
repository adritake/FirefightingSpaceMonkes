using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public bool DebugMode;

    private Fire[] _fires;

    private void Start()
    {
        _fires = FindObjectsOfType<Fire>();
    }

    private void Update()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        if(DebugMode && Input.GetKeyDown(KeyCode.R))
        {
            NetworkManager.Instance.LoadGame(); ;
        }
    }

    public bool AllFireExtinguished()
    {
        return _fires.All(x => x == null);
    }
}
