using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuCanvas : MonoBehaviour
{
    private bool _roomJoined;

    public Button createRoomButton;
    public Button creditsButton;

    public GameObject roomCanvas;
    public GameObject joinCanvas;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumberText;

    private void OnEnable()
    {
        NetworkManager.Instance.CreatedRoom += ShowRoomCanvas;
        NetworkManager.Instance.JoinedRoom += JoinedRoom;
    }


    private void Start()
    {
        _roomJoined = false;
        createRoomButton.onClick.AddListener(() => NetworkManager.Instance.CreateNewRoom());
        creditsButton.onClick.AddListener(() => PunSceneManager.Instance.LoadCreditScene());
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

    //Call LoadGame when start button is clicked
    public void StartButtonClicked()
    {
        NetworkManager.Instance.LoadGame();
    }

    //Enable Room canvas when created button is clicked
    public void ShowRoomCanvas()
    {
        roomCanvas.SetActive(true);
        JoinedRoom();
    }


    //Enable Join Room canvas when join button is clicked
    public void ShowJoinCanvas()
    {
        joinCanvas.SetActive(true);

        //if (!string.IsNullOrEmpty(_inputRoomName.text))
        //{
        //    NetworkManager.Instance.JoinRoom(_inputRoomName.text);
        //}
        //else
        //{
        //    Debug.LogError("Room name empty. Cannot join!");
        //}
    }
}
