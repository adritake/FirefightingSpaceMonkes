using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFSM
{
    public class Thruster : MonoBehaviour
    {
        public ParticleSystem Particles;
        public bool StartEnabled = false;

        private bool _particlesPlaying;

        public AudioClip thrustSound;

        private void Start()
        {
            EnableThruster(StartEnabled);
        }

        private void Update()
        {
            if (_particlesPlaying)
            {
                AudioManager.Instance.PlaySoundLoop(thrustSound);
            }
        }

        public void EnableThruster(bool enabled)
        {
            if (enabled && !_particlesPlaying)
            {
                _particlesPlaying = true;
                Particles.Play();
            }
            else if (!enabled)
            {
                _particlesPlaying = false;
                Particles.Stop();
            }
        }
    }
}
