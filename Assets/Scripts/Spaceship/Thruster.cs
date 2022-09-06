using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void EnableThruster(bool enabled)
    {
        if (enabled && !_particlesPlaying)
        {
            _particlesPlaying = true;
            AudioManager.Instance.PlaySound(thrustSound);
            Particles.Play();
        }
        else if(!enabled)
        {
            _particlesPlaying = false;
            Particles.Stop();
        }
    }
}
