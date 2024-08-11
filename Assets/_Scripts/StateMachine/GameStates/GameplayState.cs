using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Units;
using _Scripts.Utils;
using UnityEngine;
public class GameplayState : State
{

    public override void Init()
    {
        Debug.Log("Gameplay started");
        GameController.Instance.SetGameplayRefsHolder();
        GameController.Instance.RuntimeDataHolder.Reset();
        
    }

    public override void CustomUpdate()
    {
        GameController.Instance.GameplayRefsHolder.CustomUpdate();
    }

    public override void CustomFixedUpdate()
    {
    }

    public override void Deinit()
    {
        Debug.Log("Gameplay End");
        GameController.Instance.GameplayRefsHolder.Deinit();
        GameController.Instance.ClearGameplayRefsHolder();
    }
}
