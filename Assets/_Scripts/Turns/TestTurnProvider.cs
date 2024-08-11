using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TestTurnProvider : MonoBehaviour, ITurnProvider
{
    protected bool _imActive;
    public int moveCount;
    public event Action MovePerformed;

    // Update is called once per frame
    public virtual void Update()
    {
        if (!_imActive) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PerformMove();
        }
    }

    protected void PerformMove()
    {
        moveCount--;
        MovePerformed?.Invoke();
    }

    public virtual void StartMyTurn()
    {
        _imActive = true;
        moveCount = 1;
    }

    public bool HasAnyMoves()
    {
        return moveCount > 0;
    }

    public void EndMyTurn()
    {
        _imActive = false;
    }

    public virtual string GetName()
    {
        return "Enemy";
    }
}
