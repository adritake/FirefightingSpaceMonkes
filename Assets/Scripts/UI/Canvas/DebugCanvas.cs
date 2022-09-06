using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FFSM
{
    public class DebugCanvas : MonoBehaviour
    {
        [Tooltip("Bool to check if player is in a room")]
        [SerializeField] private bool _roomJoined;

        [Header("UI references")]
        public TextMeshProUGUI roomNameText;
        public TextMeshProUGUI playerNumberText;

        #region Monobehaviour
        private void OnEnable()
        {
            NetworkManager.Instance.JoinedRoom += JoinedRoom;
            NetworkManager.Instance.LeftRoom += LeftRoom;
        }

        private void OnDisable()
        {
            NetworkManager.Instance.JoinedRoom -= JoinedRoom;
            NetworkManager.Instance.LeftRoom -= LeftRoom;
        }

        private void Start()
        {
            _roomJoined = false;
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

        #region Private Methods
        private void DebugUIUpdate(string name, string playerNumber)
        {
            roomNameText.text = name;
            playerNumberText.text = playerNumber;
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
}
