using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFSM
{
    public class AudioManager : PunSingleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectSource;
        [SerializeField] private AudioSource _loopEffectSource;

        [Header("Game")]
        [Header("Music Clips")]
        public AudioClip winMusic;
        public AudioClip loseMusic;
        public AudioClip gameMusic;
        public AudioClip menuMusic;
        public AudioClip creditsMusic;

        [Header("SFX")]
        public AudioClip firesOffSound;
        public AudioClip buzzSound;

        #region Monobehaviour
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _musicSource = gameObject.transform.Find("Music Source").GetComponent<AudioSource>();
            _effectSource = gameObject.transform.Find("Effect Source").GetComponent<AudioSource>();
            _loopEffectSource = gameObject.transform.Find("Loop Effect Source").GetComponent<AudioSource>();
        }

        private void Start()
        {
            _musicSource.Play();
        }

        #endregion

        #region Generic Methods
        public void PlayMusic(AudioClip music)
        {
            _musicSource.Stop();
            _musicSource.clip = music;
            _musicSource.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            _effectSource.PlayOneShot(clip);
        }

        public void PlaySoundLoop(AudioClip clip)
        {
            _loopEffectSource.clip = clip;
            if (!_loopEffectSource.isPlaying)
            {
                _loopEffectSource.Play();
            }
        }

        #endregion

        #region Public Music Methods
        public void PlayWinMusic()
        {
            photonView.RPC(nameof(RPCPlayWinMusic), Photon.Pun.RpcTarget.AllViaServer);
        }

        public void PlayLooseMusic()
        {
            photonView.RPC(nameof(RPCPlayLoseMusic), Photon.Pun.RpcTarget.AllViaServer);
        }

        public void GameMusic()
        {
            photonView.RPC(nameof(RPCGameMusic), Photon.Pun.RpcTarget.AllViaServer);
        }

        public void CreditsMusic()
        {
            //photonView.RPC(nameof(RPCCreditsMusic), Photon.Pun.RpcTarget.AllViaServer);
            if (_musicSource.clip == creditsMusic) return;
            PlayMusic(creditsMusic);
        }

        public void MenuMusic()
        {
            //photonView.RPC(nameof(RPCMenuMusic), Photon.Pun.RpcTarget.AllViaServer);
            if (_musicSource.clip == menuMusic) return;
            PlayMusic(menuMusic);
        }

        #endregion

        #region Public SFX Methods
        public void AllFiresOffSound()
        {
            PlaySound(firesOffSound);
        }

        public void BuzzSound()
        {
            PlaySound(buzzSound);
        }

        #endregion

        #region RPCs

        [PunRPC]
        private void RPCPlayWinMusic()
        {
            PlayMusic(winMusic);
        }

        [PunRPC]
        private void RPCPlayLoseMusic()
        {
            PlayMusic(loseMusic);
        }

        [PunRPC]
        private void RPCGameMusic()
        {
            PlayMusic(gameMusic);
        }

        #endregion
    }
}
