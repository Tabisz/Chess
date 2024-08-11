using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class PlayerGameplayState : PlayerState
{
    public override void Init()
    {
        GameController.Instance.gameplayRefsHolder.InGameUIController.GetView(ViewType.TURN_VIEW).Show();
    }

    public override void Deinit()
    {
       
    }

    public override void GatherInput()
    {
        if(GameController.Instance.gameplayRefsHolder.Player.PlayerInputLocker.IsLocked) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.gameplayRefsHolder.Player.PlayerSM.ChangeState(new PlayerMenuState());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameController.Instance.gameplayRefsHolder.Player.Raycaster.TryPrimaryInteract();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameController.Instance.gameplayRefsHolder.Player.Raycaster.TrySecondaryInteract();
        }
    }
    
    

    public override void CustomUpdate()
    {
        GameController.Instance.gameplayRefsHolder.Player.Raycaster.CustomUpdate();
       GatherInput();
    }

    public override void CustomFixedUpdate()
    {
       
    }
}
