using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private bool _roomJoined;

    public Button createRoomButton;
    public Button creditsButton;

    public GameObject roomCanvas;
    public GameObject joinCanvas;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumberText;

    #region Monobehaviour
    private void OnEnable()
    {
        NetworkManager.Instance.CreatedRoom += ShowRoomCanvas;
        NetworkManager.Instance.JoinedRoom += JoinedRoom;
        NetworkManager.Instance.LeftRoom += LeftRoom;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.CreatedRoom -= ShowRoomCanvas;
        NetworkManager.Instance.JoinedRoom -= JoinedRoom;
        NetworkManager.Instance.LeftRoom -= LeftRoom;
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
        else
        {
            DebugUIUpdate("none", "000");
        }
    }

    #endregion

    #region UI Management
    private void DebugUIUpdate(string name, string playerNumber)
    {
        roomNameText.text = name;
        playerNumberText.text = playerNumber;
    }

    //Enable Room canvas when created button is clicked
    public void ShowRoomCanvas()
    {
        roomCanvas.SetActive(true);
    }


    //Enable Join Room canvas when join button is clicked
    public void ShowJoinCanvas()
    {
        joinCanvas.SetActive(true);
    }

    #endregion

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
