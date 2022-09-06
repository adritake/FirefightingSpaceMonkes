using Photon.Pun;
using UnityEngine;

namespace FFSM
{
    public class Fire : MonoBehaviourPunCallbacks
    {
        public GameObject FireLoop;
        public GameObject FireExtinguish;

        private Collider _collider;
        private bool _extinguished;

        public AudioClip extinguishSound;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Extinguish()
        {
            photonView.RPC(nameof(RPC_ExtinguishFire), RpcTarget.AllViaServer);
        }

        [PunRPC]
        private void RPC_ExtinguishFire()
        {
            if (!_extinguished)
            {
                _extinguished = true;
                LevelManager.Instance.ReduceFiresLeft();
                AudioManager.Instance.PlaySound(extinguishSound);
                FireLoop.SetActive(false);
                FireExtinguish.SetActive(true);
                _collider.enabled = false;
                Destroy(gameObject, 1);
            }
        }
    }
}
