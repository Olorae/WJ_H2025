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
        GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX(GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().btnSound);
        GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().SetMusic(GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().DeadMusic);
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
        //UnityEditor.EditorApplication.isPlaying = false;
        GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX(GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().btnSound);
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("SceneMainMenu");
    }

    public void WinScene()
    {
        GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX(GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().btnSound);
        SceneManager.LoadSceneAsync("WinScene");
    }

    public void LoseScene()
    {
        GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX(GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().btnSound);
        SceneManager.LoadSceneAsync("LoseScene");
    }
}
