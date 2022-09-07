using FFSM.GameManagers;
using UnityEngine;

namespace FFSM.UI
{
    public class JoinCanvas : MonoBehaviour
    {

        [Header("Transition Canvas References")]
        public GameObject menuCanvas;
        public GameObject roomCanvas;

        [Header("Background sprites")]
        public GameObject monkes;

        [Header("Sound")]
        public AudioClip leaveJoinSound;

        #region Public Methods
        public void MenuButtonClicked()
        {
            //Show Menu
            gameObject.SetActive(false);
            menuCanvas.SetActive(true);
            monkes.SetActive(true);
            AudioManager.Instance.PlaySound(leaveJoinSound);
        }

        public void ShowRoomCanvas()
        {
            roomCanvas.SetActive(true);
            gameObject.SetActive(false);
        }

        #endregion


    }
}
