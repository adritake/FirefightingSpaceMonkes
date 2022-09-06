using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunSceneManager : Singleton<PunSceneManager>
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
        SceneManager.LoadScene(MENU_NAME);
    }

    public void LoadCreditScene()
    {
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
            PhotonNetwork.LoadLevel(GetLevelName());
        }
    }

    public void LoadNextLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _currentLevel++;
            AudioManager.Instance.PlaySound(startSound);
            PhotonNetwork.LoadLevel(GetLevelName());
        }
    }
    #endregion

    #region Private methods
    private string GetLevelName()
    {
        return LEVEL_NAME + _currentLevel.ToString("00");
    }
    #endregion
}

