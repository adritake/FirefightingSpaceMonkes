using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ListedRoom : MonoBehaviour
{
    [Header("Listed Room properties")]
    [SerializeField] private TextMeshProUGUI _listedRoomText;
    public RoomInfo RoomInfo { get; private set; }

    [Header("Join Canvas reference")]
    public JoinCanvas joinCanvas;

    [Header("Button Sound")]
    public AudioClip buttonSound;

    private void Start()
    {
        joinCanvas = FindObjectOfType<JoinCanvas>();
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _listedRoomText.text = roomInfo.Name + " - " + roomInfo.PlayerCount + " i " + roomInfo.MaxPlayers;
    }

    public void JoinSelectedRoom()
    {
        AudioManager.Instance.PlaySound(buttonSound);
        joinCanvas.ShowRoomCanvas();
        NetworkManager.Instance.JoinRoom(RoomInfo.Name);
    }
}
