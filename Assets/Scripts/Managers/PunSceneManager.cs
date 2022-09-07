using FFSM.Utils;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FFSM.GameManagers
{
    public class PunSceneManager : PunSingleton<PunSceneManager>
    {
        private int _currentLevel;
        private const string LEVEL_NAME = "Level";
        private const string CREDIT_NAME = "Credits";
        private const string MENU_NAME = "1_MenuScene";
        private const string UTILITY_SCENE_NAME = "ReloadSceneUtility";

        public AudioClip startSound;

        #region MonoBehaviour
        private void Start()
        {
            _currentLevel = 0;
            DontDestroyOnLoad(this);
        }
        #endregion

        #region Public methods
        public void LoadMenuScene()
        {
            AudioManager.Instance.MenuMusic();
            SceneManager.LoadScene(MENU_NAME);
        }

        public void LoadCreditScene()
        {
            AudioManager.Instance.CreditsMusic();
            SceneManager.LoadScene(CREDIT_NAME);
        }

        public void ReloadLevel()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(UTILITY_SCENE_NAME);
            }
        }

        public void LoadCurrentLevel()
        {
            if (PhotonNetwork.IsMasterClient)

            {
                AudioManager.Instance.PlaySound(startSound);
                AudioManager.Instance.GameMusic();
                PhotonNetwork.LoadLevel(GetLevelName());
            }
        }

        public void LoadNextLevel()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                IncreaseLevel();
                AudioManager.Instance.PlaySound(startSound);
                AudioManager.Instance.GameMusic();
                PhotonNetwork.LoadLevel(GetLevelName());
            }
        }
        #endregion

        #region Private methods
        private string GetLevelName()
        {
            return LEVEL_NAME + _currentLevel.ToString("00");
        }

        private void IncreaseLevel()
        {
            _currentLevel++;
            photonView.RPC(nameof(RPC_IncreaseLevel), RpcTarget.Others);
        }
        #endregion

        #region RPC
        [PunRPC]
        private void RPC_IncreaseLevel()
        {
            _currentLevel++;
        }
        #endregion
    }
}

