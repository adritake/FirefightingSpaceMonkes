using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte _coopPlayers = 2;

    [SerializeField] private string _gameVersion = "1.0.0";

    public static NetworkManager Instance;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Login()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("[Network manager]: Connected to " + PhotonNetwork.CloudRegion + " server");
        //Load Main Menu Scene
        SceneManager.LoadScene(1);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[Network manager]: Disconnected. Reason: " + cause);
        //En caso de que se caiga, se puede volver a pedir reconexión después de timeout se puede llamar con un tiempo de espera exponencial
    }

    #endregion

    #region Room Management

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

    public void LoadMatch()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            Debug.Log("[Network Manager]: Loading match level");
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("[Network Manager]: Join random failed. No random room available, proceeding to create a new one");
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions { MaxPlayers = _coopPlayers });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[Network Manager]: Joined Room " + PhotonNetwork.CurrentRoom.Name);
    }

    #endregion
}
