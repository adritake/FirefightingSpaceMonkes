using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PunSingleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;

    #region Monobehaviour
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _musicSource = gameObject.transform.Find("Music Source").GetComponent<AudioSource>();
        _effectSource = gameObject.transform.Find("Effect Source").GetComponent<AudioSource>();
    }

    private void Start()
    {
        _musicSource.Play();
    }

    #endregion

    public void PlayMusic(AudioClip music)
    {
        _musicSource.Stop();
        _musicSource.PlayOneShot(music);
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

}
