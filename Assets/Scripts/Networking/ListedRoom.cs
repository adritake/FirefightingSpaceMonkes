using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ListedRoom : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _listedRoomText;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _listedRoomText.text = roomInfo.Name + " - " + roomInfo.PlayerCount + " i " + roomInfo.MaxPlayers;
    }
}
