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
        GameController.Instance.gameplayRefsHolder = Object.FindFirstObjectByType<GameplayRefsHolder>();
        GameController.Instance.gameplayRefsHolder.Init();
        GameController.Instance.GlobalStatistics.ClearKilledCount();
        GameController.Instance.gameplayRefsHolder.Observer.OnUnitDied += OnKilled;


    }

    private void OnKilled(Unit unit)
    {
        if(unit.Fraction == Fraction.ENEMY)
            GameController.Instance.GlobalStatistics.IncrementKilledCount();
    }

    public override void CustomUpdate()
    {
        GameController.Instance.gameplayRefsHolder.CustomUpdate();
    }

    public override void CustomFixedUpdate()
    {
    }

    public override void Deinit()
    {
        Debug.Log("Gameplay End");
        GameController.Instance.gameplayRefsHolder.Observer.OnUnitDied -= OnKilled;
        GameController.Instance.gameplayRefsHolder.Deinit();
        GameController.Instance.gameplayRefsHolder = null;


    }
}
