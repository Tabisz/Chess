using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Units;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameplayState : State
{


    public override void Init()
    {
        Debug.Log("Gameplay started");
        GameController.Instance.GameplayRefsHolder = GameObject.FindObjectOfType<GameplayRefsHolder>();
        GameController.Instance.GameplayRefsHolder.Init();
        GameController.Instance.GlobalStatistics.ClearKilledCount();
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied += OnKilled;


    }

    private void OnKilled(Unit unit)
    {
        if(unit.Fraction == Fraction.ENEMY)
            GameController.Instance.GlobalStatistics.IncrementKilledCount();
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
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied -= OnKilled;
        GameController.Instance.GameplayRefsHolder.Deinit();
        GameController.Instance.GameplayRefsHolder = null;


    }
}
