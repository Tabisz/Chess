using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTurnProvider : TestTurnProvider
{
    void Update()
    {
        if (!_imActive) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            PerformMove();
        }
    }

    public override void StartMyTurn()
    {
        Debug.Log("Im SPECIAL turn provider");
        base.StartMyTurn();
    }
    public override string GetName()
    {
        return "Special";
    }
}
