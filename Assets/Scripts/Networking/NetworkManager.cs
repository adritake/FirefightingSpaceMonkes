using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;

public class NetworkManager : PunSingleton<NetworkManager>
{
    [SerializeField] private byte _coopPlayers = 2;
    private int _roomIndex = 0;

    [SerializeField] private string _gameVersion = "1.0.0";

    public event Action CreatedRoom;
    public event Action JoinedRoom;
    public event Action LeftRoom;
    public event Action<Player> OtherJoinRoom;
    public event Action OtherLeftRoom;

    #region Monobehaviour
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

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
        PunSceneManager.Instance.LoadMenuScene();   
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to the lobby");
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
        _roomIndex++;
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "_" + _roomIndex, new RoomOptions { MaxPlayers = _coopPlayers });
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        StartCoroutine(LeaveRoomCoroutine());
    }

    private IEnumerator LeaveRoomCoroutine()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LeaveRoom();
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
        Debug.Log("[Network Manager]: Left Room");
        LeftRoom?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("[Netwoek Manager]: Player " + newPlayer.NickName + " entered the room");
        OtherJoinRoom?.Invoke(newPlayer);

        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers) PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("[Netwoek Manager]: Player " + otherPlayer.NickName + " left the room");
        OtherLeftRoom?.Invoke();
        if(PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers) PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    #endregion

    #region Getters

    public bool IsPlayerMaster()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public Player GetMasterPlayer()
    {
        return PhotonNetwork.MasterClient;
    }

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

    public Player GetLocalPlayer()
    {
        return PhotonNetwork.LocalPlayer;
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
