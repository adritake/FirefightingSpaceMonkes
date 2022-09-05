using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class RoomCanvas : MonoBehaviour
{
    public GameObject menuCanvas;
    public Button startButton;
    private bool readyToStart = false;

    public TextMeshProUGUI joinRoomText;
    public TextMeshProUGUI player1Name;
    public TextMeshProUGUI player2Name;

    public Image player2Sprite;

    private void OnEnable()
    {
        NetworkManager.Instance.JoinedRoom += PlayerJoined;
        NetworkManager.Instance.OtherJoinRoom += OtherPlayerJoined;
        NetworkManager.Instance.OtherLeftRoom += OtherPlayerLeft;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.JoinedRoom -= PlayerJoined;
        NetworkManager.Instance.OtherJoinRoom -= OtherPlayerJoined;
        NetworkManager.Instance.OtherLeftRoom -= OtherPlayerLeft;
    }

    private void Start()
    {
        joinRoomText.text = "waiting for a monke comrade...";
    }

    private void Update()
    {
        if(NetworkManager.Instance.IsPlayerMaster() && readyToStart)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void MenuButtonClicked()
    {
        //Show Menu & Left Room
        NetworkManager.Instance.LeaveRoom();
        gameObject.SetActive(false);
    }

    //Call LoadGame when start button is clicked
    public void StartButtonClicked()
    {
        NetworkManager.Instance.LoadGame();
    }

    private void PlayerJoined()
    {
        if (NetworkManager.Instance.IsPlayerMaster())
        {
            player1Name.text = NetworkManager.Instance.GetLocalPlayer().NickName;
        }
        else
        {
            player1Name.text = NetworkManager.Instance.GetMasterPlayer().NickName;
            OtherPlayerJoined(NetworkManager.Instance.GetLocalPlayer());
        }
    }

    private void OtherPlayerJoined(Player player)
    {
        readyToStart = true;
        joinRoomText.text = "ready for take off!";
        player2Sprite.color = Color.white;
        player2Name.gameObject.SetActive(true);
        player2Name.text = player.NickName;
}

    private void OtherPlayerLeft()
    {
        if (NetworkManager.Instance.IsPlayerMaster())
        {
            player1Name.text = NetworkManager.Instance.GetLocalPlayer().NickName;
        }
        readyToStart = false;
        joinRoomText.text = "waiting for a monke comrade...";
        player2Sprite.color = Color.black;
        player2Name.gameObject.SetActive(false);
        player2Name.text = "player 2";
    }
}
