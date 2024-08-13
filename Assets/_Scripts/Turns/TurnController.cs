using System;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Turns
{
    public class TurnController : MonoBehaviour, ICustomInitializer
    {
        private int _currentTurn;
        public int CurrentTurn => _currentTurn;
    
        private int _enemyTurnsToSpawning;
    

        private ITurnProvider _currentTurnProvider;
        public ITurnProvider CurrentTurnProvider => _currentTurnProvider;

        private ITurnProvider _playerTurnProvider;
        private ITurnProvider _aiTurnProvider;
    
        private ITurnProvider _spawningTurnProvider;

        public Action<int,string> OnNextTurn;
        public Action OnNextPhase;
    
        public void Init()
        {
            _currentTurn = 0;
            _enemyTurnsToSpawning = 3;
            NextTurn();
        }

        public void PlayerRegisterTurnProvider(ITurnProvider provider)
        {
            _playerTurnProvider = provider;
        }

        public void SpawningRegisterTurnProvider(ITurnProvider provider)
        {
            _spawningTurnProvider = provider;
        }

        public void AiRegisterTurnProvider(ITurnProvider provider)
        {
            _aiTurnProvider = provider;
        }

    
    
        public void CustomUpdate()
        {
        
        }

        private void TryNextTurn()
        {
            if (!_currentTurnProvider.HasAnyMoves())
            {
                NextTurn();
            }
        }
    
        private void NextTurn()
        {
            SwitchProvider(GetNextProvider());
            OnNextTurn?.Invoke(_currentTurn,_currentTurnProvider.GetName());
        }
        private void SwitchProvider(ITurnProvider newTurnProvider)
        {
            if (_currentTurnProvider != null)
            {
                _currentTurnProvider.EndMyTurn();
                _currentTurnProvider.MovePerformed -= TryNextTurn;
            }

            _currentTurnProvider = newTurnProvider;

        
            _currentTurnProvider.MovePerformed += TryNextTurn;
                
            _currentTurnProvider.StartMyTurn();

        
        }
        private ITurnProvider GetNextProvider()
        {
            if (_currentTurnProvider == null)
            {
                _currentTurn++;
                return _playerTurnProvider;
            }

            if (_currentTurnProvider.GetName() == _aiTurnProvider.GetName())
            {
                _currentTurn++;
                _enemyTurnsToSpawning--;
                return _playerTurnProvider;
            }

            if (_currentTurnProvider.GetName() == _playerTurnProvider.GetName())
            {
                if (_enemyTurnsToSpawning == 0)
                {
                    _enemyTurnsToSpawning = 3;
                    return _spawningTurnProvider;
                }
                else
                    return _aiTurnProvider;
            }
            if (_currentTurnProvider.GetName() == _spawningTurnProvider.GetName())
            {
                return _aiTurnProvider;
            }
        
            return _playerTurnProvider;
        }

        public void RequestSkip(string requester)
        {
            if(requester == _currentTurnProvider.GetName())
                NextTurn();
        }

        public void Deinit()
        {
        }
    }
}
