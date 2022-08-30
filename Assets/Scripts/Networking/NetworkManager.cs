using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkManager : PunSingleton<NetworkManager>
{
    [SerializeField] private byte _coopPlayers = 2;

    [SerializeField] private string _gameVersion = "1.0.0";

    public event Action CreatedRoom;
    public event Action JoinedRoom;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);
    }

    #region Login & Nickname
    public void Login()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }
    public void SetPlayerName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    #endregion

    #region PUN Master Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("[Network manager]: Connected to " + PhotonNetwork.CloudRegion + " server");
        //Load Main Menu Scene
        Debug.Log("Player name is: " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();      
    }

    public override void OnJoinedLobby()
    {
        PunSceneManager.Instance.LoadMenuScene();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[Network manager]: Disconnected. Reason: " + cause);
        //En caso de que se caiga, se puede volver a pedir reconexión después de timeout se puede llamar con un tiempo de espera exponencial
    }

    #endregion

    #region Room Management

    public void CreateNewRoom()
    {
        if (!PhotonNetwork.IsConnected) return;
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions { MaxPlayers = _coopPlayers });
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void QuickPlay()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("[NetworkManager]: Not Connected");
            Login();
        }
    }

    public void LoadGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            //Condition to be joinable (list)
            //PhotonNetwork.CurrentRoom.IsOpen = false;

            Debug.Log("[Network Manager]: Loading match level");

            //Method called when joining a game 
            //PhotonNetwork.LoadLevel("Level_" + 1);
            PunSceneManager.Instance.LoadNextLevel();
        }
    }

    #endregion

    #region PUN Room Methods Override

    public override void OnCreatedRoom()
    {
        Debug.Log("[Network Manager]: Created Room " + PhotonNetwork.CurrentRoom.Name);
        //call event to enable start game (later it will be a new canvas with host player waiting for 2nd player
        CreatedRoom?.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("[Network Manager]: Create room failed. Error: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[Network Manager]: Joined Room " + PhotonNetwork.CurrentRoom.Name);
        JoinedRoom?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("[Network Manager]: Join random failed. Error: " + message);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    #endregion

    #region Debug Data

    public string GetRoomName()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            return PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            Debug.LogWarning("No room joined yet");
            return "N/A";
        }
    }

    public string GetPlayersInRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            return "1";
        }
        else
        {
            return PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    #endregion
}
