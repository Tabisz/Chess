using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class PlayerMenuState : PlayerState
{
    public override void Init()
    {
        GameController.Instance.gameplayRefsHolder.InGameUIController.GetView(ViewType.IN_GAME_MENU_VIEW).Show();
    }
    public override void CustomUpdate()
    {
        GatherInput();
    }
    public override void GatherInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.gameplayRefsHolder.Player.PlayerSM.ChangeState(new PlayerGameplayState());            
        }
    }

    public override void CustomFixedUpdate()
    {
    }
    
    public override void Deinit()
    {
        GameController.Instance.gameplayRefsHolder.InGameUIController.GetView(ViewType.IN_GAME_MENU_VIEW).Hide();
    }

}
