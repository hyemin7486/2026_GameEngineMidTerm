using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public GameObject HelpPanel;
    public GameObject ScorePanel;
    public void GameStartButtonAction()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OpenHelpPanel()
    {
        HelpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        HelpPanel.SetActive(false);
    }
    public void OpenScorePanel()
    {
        ScorePanel.SetActive(true);
    }

    public void CloseScorePanel()
    {
        ScorePanel.SetActive(false);
    }

    public void ExitGame()
    {
         print("Exit Game");
     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
      Application.Quit();
     #endif
    }
}
