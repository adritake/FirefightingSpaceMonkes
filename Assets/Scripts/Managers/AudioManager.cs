using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PunSingleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;

    [Header("Game")]
    [Header("Music Clips")]
    public AudioClip winMusic;
    public AudioClip looseMusic;
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

    #endregion

    #region Game Music
    public void PlayWinMusic()
    {
        PlayMusic(winMusic);
    }

    public void PlayLooseMusic()
    {
        PlayMusic(looseMusic);
    }

    public void GameMusic()
    {
        PlayMusic(gameMusic);
    }

    public void CreditsMusic()
    {
        if (_musicSource.clip == creditsMusic) return;
        PlayMusic(creditsMusic);
    }

    public void MenuMusic()
    {
        if (_musicSource.clip == menuMusic) return;

        PlayMusic(menuMusic);
    }

    #endregion

    #region SFX
    public void AllFiresOffSound()
    {
        PlaySound(firesOffSound);
    }

    public void BuzzSound()
    {
        PlaySound(buzzSound);
    }

    #endregion

}
