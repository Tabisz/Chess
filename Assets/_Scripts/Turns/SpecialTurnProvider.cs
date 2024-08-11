using UnityEngine;

public class SpecialTurnProvider : TestTurnProvider
{
    public override void Update()
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
