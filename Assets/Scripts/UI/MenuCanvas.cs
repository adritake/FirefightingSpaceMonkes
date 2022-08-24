using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuCanvas : MonoBehaviour
{
    private bool _roomJoined;

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button creditsButton;
    public Button exitButton;
    public Button startButton;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumberText;
    public TMP_InputField _inputRoomName;

    private void OnEnable()
    {
        NetworkManager.Instance.CreatedRoom += ShowStartButton;
        NetworkManager.Instance.JoinedRoom += JoinedRoom;
    }


    private void Start()
    {
        _roomJoined = false;
        createRoomButton.onClick.AddListener(() => NetworkManager.Instance.CreateNewRoom());
    }

    private void Update()
    {
        if (_roomJoined)
        {
            DebugUIUpdate(NetworkManager.Instance.GetRoomName(), NetworkManager.Instance.GetPlayersInRoom());
        }
    }

    private void DebugUIUpdate(string name, string playerNumber)
    {
        roomNameText.text = name;
        playerNumberText.text = playerNumber;
    }
    private void JoinedRoom()
    {
        _roomJoined = true;
    }

    public void JoinRoomButtonClicked()
    {
        if (!string.IsNullOrEmpty(_inputRoomName.text))
        {
            NetworkManager.Instance.JoinRoom(_inputRoomName.text);
        }
        else
        {
            Debug.LogError("Room name empty. Cannot join!");
        }
    }

    public void StartGameCreated()
    {
        NetworkManager.Instance.LoadGame(); ;
    }

    //Enable Start button when created room
    public void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
        JoinedRoom();
    }

    //Call LoadGame when start button is created

    public void StartButtonClicked()
    {
        NetworkManager.Instance.LoadGame();
    }
}
