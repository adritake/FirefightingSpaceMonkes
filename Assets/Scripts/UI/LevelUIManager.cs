using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [Header("Win")]
    public GameObject WinCanvas;
    public GameObject NextLevelButton;

    [Header("Loose")]
    public GameObject LooseCanvas;
    public GameObject RetryButton;

    [Header("Tutorial")]
    public TutorialText TutorialText;

    #region Monobehaviour
    void Update()
    {
        CheckHideTutorial();
    }
    #endregion

    #region Public methods
    public void EnableWinCanvas(bool enabled, bool enableButton)
    {
        WinCanvas.SetActive(enabled);
        NextLevelButton.SetActive(enableButton);
    }

    public void EnableLooseCanvas(bool enabled, bool enableButton)
    {
        LooseCanvas.SetActive(enabled);
        RetryButton.SetActive(enableButton);
        TutorialText.Hide();
    }
    #endregion

    #region Private methods
    private void CheckHideTutorial()
    {
        if (TutorialText && Input.GetKeyDown(KeyCode.Space))
        {
            TutorialText.Hide();
        }
    }
    #endregion

    #region Events
    public void OnRetryButtonClicked()
    {
        PunSceneManager.Instance.ReloadLevel();
    }

    public void OnNextLevelButtonClicked()
    {
        PunSceneManager.Instance.LoadNextLevel();
    }
    #endregion
}
