using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using FFSM.Network;
using FFSM.GameManagers;

namespace FFSM.UI
{
    public class RoomCanvas : MonoBehaviour
    {
        [Header("Menu Buttons References")]
        public Button startButton;
        public Button menuButton;

        [Header("Transition Canvas References")]
        public GameObject menuCanvas;

        [Header("Background Sprite Reference")]
        public GameObject monkes;

        [Header("UI References")]
        public TextMeshProUGUI joinRoomText;
        public TextMeshProUGUI player1Name;
        public TextMeshProUGUI player2Name;
        public Image player2Sprite;

        [Header("Sound")]
        public AudioClip leaveRoomSound;
        public AudioClip playerJoinedSound;

        private bool readyToStart = false;

        #region Monobehaviour
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
            menuButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (NetworkManager.Instance.IsPlayerMaster() && readyToStart)
            {
                startButton.gameObject.SetActive(true);
            }
            else
            {
                startButton.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Public Methods
        public void MenuButtonClicked()
        {
            //Show Menu & Left Room
            NetworkManager.Instance.LeaveRoom();
            gameObject.SetActive(false);
            menuCanvas.SetActive(true);
            monkes.SetActive(true);
            AudioManager.Instance.PlaySound(leaveRoomSound);
        }

        //Call LoadGame when start button is clicked
        public void StartButtonClicked()
        {
            NetworkManager.Instance.LoadGame();
        }
        #endregion

        #region Private Network Methods
        private void PlayerJoined()
        {
            if (NetworkManager.Instance.IsPlayerMaster())
            {
                player1Name.text = NetworkManager.Instance.GetLocalPlayer().NickName;
                menuButton.gameObject.SetActive(true);
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
            menuButton.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(playerJoinedSound);
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
            AudioManager.Instance.PlaySound(leaveRoomSound);
        }

        #endregion
    }
}
