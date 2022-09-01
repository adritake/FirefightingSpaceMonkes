using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : PunSingleton<LevelManager>
{
    public bool DebugMode;

    private Fire[] _fires;

    private void Start()
    {
        _fires = FindObjectsOfType<Fire>();
    }

    #region Public methods
    public bool AllFireExtinguished()
    {
        return _fires.All(x => x == null);
    }

    public void CompleteLevel()
    {
        photonView.RPC(nameof(RPC_CompleteLevel), RpcTarget.AllViaServer);
    }

    public void FailLevel()
    {
        photonView.RPC(nameof(RPC_LooseLevel), RpcTarget.AllViaServer);
    }
    #endregion

    #region RPC
    [PunRPC]
    private void RPC_CompleteLevel()
    {
        LevelUIManager.Instance.EnableWinCanvas(true, PhotonNetwork.IsMasterClient);
    }

    [PunRPC]
    private void RPC_LooseLevel()
    {
        LevelUIManager.Instance.EnableLooseCanvas(true, PhotonNetwork.IsMasterClient);
    }
    #endregion
}
