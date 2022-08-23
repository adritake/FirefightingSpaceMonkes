using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private Fire[] _fires;

    private void Start()
    {
        _fires = FindObjectsOfType<Fire>();
    }

    public bool AllFireExtinguished()
    {
        return _fires.All(x => x == null);
    }
}
