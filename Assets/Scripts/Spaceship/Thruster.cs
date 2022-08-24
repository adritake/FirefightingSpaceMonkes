using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public ParticleSystem Particles;

    private bool _particlesPlaying;

    private void Start()
    {
        EnableThruster(false);
    }

    public void EnableThruster(bool enabled)
    {
        if (enabled && !_particlesPlaying)
        {
            _particlesPlaying = true;
            Particles.Play();
        }
        else if(!enabled)
        {
            _particlesPlaying = false;
            Particles.Stop();
        }
    }
}