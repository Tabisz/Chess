using System.Collections;
using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;

public class StateMachine<T> : ICustomUpdater where T:State
{
    private State _currentState;
    public State CurrentState => _currentState;
    
    public void Init()
    {
    }

    public void CustomUpdate()
    {
        _currentState.CustomUpdate();
    }

    public void CustomFixedUpdate()
    {
        _currentState.CustomFixedUpdate();
    }
    public void ChangeState(State newState)
    {
        if(_currentState!= null)
            _currentState.Deinit();

        _currentState = newState;
        newState.Init();
    }

    public void Deinit()
    {
        _currentState.Deinit();
    }
}
