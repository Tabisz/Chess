using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Utils;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour, ICustomInitializer
{
    [SerializeField]
    private Transform mainPanel;
    [SerializeField]
    private Transform difficultyPanel;
    [SerializeField]
    private Transform exitPanel;
    
    public void Init()
    {
        ShowMainPanel();
    }
    
    public void ShowMainPanel()
    {
        mainPanel.gameObject.SetActive(true);
        difficultyPanel.gameObject.SetActive(false);
        exitPanel.gameObject.SetActive(false);
    }

    public void ShowDifficultyPanel()
    {
        mainPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(true);
        exitPanel.gameObject.SetActive(false);
    }

    public void ShowExitPanel()
    {
        mainPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        exitPanel.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        GameController.Instance.SceneLoader.LoadScene(SceneEnum.GAMEPLAY,
            () => GameController.Instance.GameSm.ChangeState(new NullState()),
            () => GameController.Instance.GameSm.ChangeState(new GameplayState()));
    }
    
    public  void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void SetEasyDifficulty()
    {
        GameController.Instance.GlobalStatistics.SetDifficultySettings(DifficultySetting.EASY);
    }
    public void SetMediumDifficulty()
    {
        GameController.Instance.GlobalStatistics.SetDifficultySettings(DifficultySetting.MEDIUM);
    }
    public void SetHardDifficulty()
    {
        GameController.Instance.GlobalStatistics.SetDifficultySettings(DifficultySetting.HARD);
    }
    public void Deinit()
    {
        
    }
}
