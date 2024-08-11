using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class PlayerGameplayState : PlayerState
{
    public override void Init()
    {
        if(GameController.Instance.GameplayRefsHolder)
        GameController.Instance.GameplayRefsHolder.InGameUIController.GetView(ViewType.TURN_VIEW).Show();
    }

    public override void Deinit()
    {
       
    }

    public override void GatherInput()
    {
        if(GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.IsLocked) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.GameplayRefsHolder.Player.PlayerSM.ChangeState(new PlayerMenuState());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameController.Instance.GameplayRefsHolder.Player.Raycaster.TryPrimaryInteract();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameController.Instance.GameplayRefsHolder.Player.Raycaster.TrySecondaryInteract();
        }
    }
    
    

    public override void CustomUpdate()
    {
        GameController.Instance.GameplayRefsHolder.Player.Raycaster.CustomUpdate();
       GatherInput();
    }

    public override void CustomFixedUpdate()
    {
       
    }
}
