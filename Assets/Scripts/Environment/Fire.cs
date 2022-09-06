using Photon.Pun;
using UnityEngine;

public class Fire : MonoBehaviourPunCallbacks
{
    public GameObject FireLoop;
    public GameObject FireExtinguish;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Extinguish()
    {
        photonView.RPC(nameof(RPC_ExtinguishFire), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_ExtinguishFire()
    {
        FireLoop.SetActive(false);
        FireExtinguish.SetActive(true);
        _collider.enabled = false;
        Destroy(gameObject, 1);
    }
} 
