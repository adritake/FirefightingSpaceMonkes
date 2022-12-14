using FFSM.GameManagers;
using UnityEngine;

namespace FFSM.UI
{
    public class ExitCredits : MonoBehaviour
    {
        [Header("Sound for Menu button")]
        public AudioClip menuSound;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMenu();
            }
        }

        #region Public Methods
        public void LoadMenu()
        {
            AudioManager.Instance.PlaySound(menuSound);
            AudioManager.Instance.MenuMusic();
            PunSceneManager.Instance.LoadMenuScene();
        }

        #endregion
    }
}
