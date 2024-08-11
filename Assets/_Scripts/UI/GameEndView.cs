using System.Collections;
using System.Collections.Generic;
using _Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameEndView : View
{

    public TMP_Text killedCountText;
    public override void Init()
    {
        base.Init();
        GameController.Instance.GameplayRefsHolder.Observer.OnGameLost += Show;
    }

    public override void Show()
    {
        killedCountText.text = "Killed count: " + GameController.Instance.GlobalStatistics.GetKilledCount();
        base.Show();
    }

    public void Restart()
    {
        GameController.Instance.SceneLoader.LoadScene(SceneEnum.GAMEPLAY,
            () => GameController.Instance.GameSm.ChangeState(new NullState()),
            () => GameController.Instance.GameSm.ChangeState(new GameplayState()));
    }
    public void ReturnToMenu()
    {
        GameController.Instance.SceneLoader.LoadScene(SceneEnum.MENU,
            () => GameController.Instance.GameSm.ChangeState(new NullState()),
            () => GameController.Instance.GameSm.ChangeState(new MenuState()));
    }

    public override void Deinit()
    {
        GameController.Instance.GameplayRefsHolder.Observer.OnGameLost -= Show;
    }
}
