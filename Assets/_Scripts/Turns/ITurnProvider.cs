using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnProvider
{
    event Action MovePerformed;

    void StartMyTurn();
    bool HasAnyMoves();
    void EndMyTurn();

    string GetName();
}
