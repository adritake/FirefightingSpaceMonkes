using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity);
    }


}
