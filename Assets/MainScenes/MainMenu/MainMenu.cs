using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
    public void GuideScene()
    {
       // SceneManager.LoadSceneAsync("OptionScene");
    }
    public void CreditsScene()
    {
        // SceneManager.LoadSceneAsync("CreditsScene");
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        QuitGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("SceneMainMenu");
    }

    public void WinScene()
    {
        SceneManager.LoadSceneAsync("WinScene");
    }

    public void LoseScene()
    {
        SceneManager.LoadSceneAsync("LoseScene");
    }
}
