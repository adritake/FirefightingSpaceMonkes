using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviourPunCallbacks
{
    public GameObject FireLoop;
    public GameObject FireExtinguish;

    public void Extinguish()
    {
        photonView.RPC(nameof(RPC_ExtinguishFire), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_ExtinguishFire()
    {
        FireLoop.SetActive(false);
        FireExtinguish.SetActive(true);
        Destroy(gameObject, 2);
    }
}
