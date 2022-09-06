using Photon.Pun;
using System.Linq;
using UnityEngine;

namespace FFSM
{
    public class LevelManager : PunSingleton<LevelManager>
    {
        public bool DebugMode;

        private int _firesLeft;

        private void Start()
        {
            _firesLeft = FindObjectsOfType<Fire>().Count();
            LevelUIManager.Instance.SetRemainingFires(_firesLeft);
        }

        #region Public methods
        public bool AllFireExtinguished()
        {
            return _firesLeft <= 0;
        }

        public void ReduceFiresLeft()
        {
            _firesLeft--;
            LevelUIManager.Instance.SetRemainingFires(_firesLeft);

            if (_firesLeft <= 0)
            {
                AudioManager.Instance.AllFiresOffSound();
            }
        }

        public void WarningFires()
        {
            LevelUIManager.Instance.WarningFire();
        }

        public void CompleteLevel()
        {
            photonView.RPC(nameof(RPC_CompleteLevel), RpcTarget.AllViaServer);
        }

        public void FailLevel()
        {
            photonView.RPC(nameof(RPC_LooseLevel), RpcTarget.AllViaServer);
        }
        #endregion

        #region RPC
        [PunRPC]
        private void RPC_CompleteLevel()
        {
            AudioManager.Instance.PlayWinMusic();
            LevelUIManager.Instance.EnableWinCanvas(true, PhotonNetwork.IsMasterClient);
        }

        [PunRPC]
        private void RPC_LooseLevel()
        {
            AudioManager.Instance.PlayLooseMusic();
            LevelUIManager.Instance.EnableLooseCanvas(true, PhotonNetwork.IsMasterClient);
        }
        #endregion
    }
}
