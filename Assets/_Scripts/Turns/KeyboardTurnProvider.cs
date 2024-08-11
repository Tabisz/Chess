using UnityEngine;

public class KeyboardTurnProvider : TestTurnProvider
{
    public override void Update()
    {
        if (!_imActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformMove();
        }
    }

    public override string GetName()
    {
        return "Keyboard";
    }
}

