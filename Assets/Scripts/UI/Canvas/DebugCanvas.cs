using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private bool _roomJoined;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumberText;

    private void OnEnable()
    {
        NetworkManager.Instance.JoinedRoom += JoinedRoom;
        NetworkManager.Instance.LeftRoom += LeftRoom;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.JoinedRoom -= JoinedRoom;
        NetworkManager.Instance.LeftRoom -= LeftRoom;
    }

    private void Start()
    {
        _roomJoined = false;
    }

    private void Update()
    {
        if (_roomJoined)
        {
            DebugUIUpdate(NetworkManager.Instance.GetRoomName(), NetworkManager.Instance.GetPlayersInRoom());
        }
        else
        {
            DebugUIUpdate("none", "000");
        }
    }

    private void DebugUIUpdate(string name, string playerNumber)
    {
        roomNameText.text = name;
        playerNumberText.text = playerNumber;
    }

    #region Setters
    private void JoinedRoom()
    {
        _roomJoined = true;
    }

    private void LeftRoom()
    {
        _roomJoined = false;
    }

    #endregion
}
