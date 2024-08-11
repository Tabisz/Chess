using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTurnProvider : TestTurnProvider
{

    void Update()
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

