using UnityEngine;
using UnityEngine.UI;
using FFSM.Network;
using FFSM.GameManagers;

namespace FFSM.UI
{
    public class MenuCanvas : MonoBehaviour
    {
        [Header("Menu Buttons References")]
        public Button createRoomButton;
        public Button joinRoomButton;
        public Button creditsButton;

        [Header("Transition Canvas References")]
        public GameObject roomCanvas;
        public GameObject joinCanvas;

        [Header("Background Sprites")]
        public GameObject monkes;

        [Header("Sound")]
        public AudioClip buttonClip;

        #region Monobehaviour
        private void OnEnable()
        {
            NetworkManager.Instance.CreatedRoom += ShowRoomCanvas;
        }

        private void OnDisable()
        {
            NetworkManager.Instance.CreatedRoom -= ShowRoomCanvas;
        }

        private void Start()
        {
            createRoomButton.onClick.AddListener(() => NetworkManager.Instance.CreateNewRoom());
            creditsButton.onClick.AddListener(() => PunSceneManager.Instance.LoadCreditScene());

            createRoomButton.onClick.AddListener(() => AudioManager.Instance.PlaySound(buttonClip));
            joinRoomButton.onClick.AddListener(() => AudioManager.Instance.PlaySound(buttonClip));
            creditsButton.onClick.AddListener(() => AudioManager.Instance.PlaySound(buttonClip));
        }

        #endregion

        #region UI Management

        //Enable Room canvas when created button is clicked
        public void ShowRoomCanvas()
        {
            gameObject.SetActive(false);
            roomCanvas.SetActive(true);
            monkes.SetActive(false);
        }


        //Enable Join Room canvas when join button is clicked
        public void ShowJoinCanvas()
        {
            gameObject.SetActive(false);
            joinCanvas.SetActive(true);
            monkes.SetActive(false);
        }

        #endregion
    }
}
