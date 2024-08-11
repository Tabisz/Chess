using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class InGameMenuView : View
{
    public void ExitToMenu()
    {
        GameController.Instance.SceneLoader.LoadScene(SceneEnum.MENU,
            () => GameController.Instance.GameSm.ChangeState(new NullState()),
            () => GameController.Instance.GameSm.ChangeState(new MenuState()));
    }
    public void ReturnToGame()
    {
        Hide();
    }
    
}
