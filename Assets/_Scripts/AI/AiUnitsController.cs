using System;
using _Scripts.Units;

namespace _Scripts.AI
{
    public class AiUnitsController : UnitsController, ITurnProvider
    {
        public event Action MovePerformed;

        private int _currentUnitMoving;

        public override void Init()
        {
            base.Init();
            TurnController.AiRegisterTurnProvider(this);
            
            Observer.OnUnitDied += CheckForGameWin;

        }

        public void StartMyTurn()
        {
            _currentUnitMoving = 0;
            Observer.OnNextMoveRequested += MoveOneUnit;
        }

        private void MoveOneUnit()
        {
            var unit = units[_currentUnitMoving];

            if (unit.CanPerformAttack() && TryAttackIfInRange(unit)) //try to attack
            {
                _currentUnitMoving++;
                unit.SkipTurn();
                MovePerformed?.Invoke();
                return;
            }

            if (unit.CanPerformMove()) //try to move
            {
                TryMoveTowardsClosestPlayer(unit);
                MovePerformed?.Invoke();
            }
            else// do nothing
            {
                unit.SkipTurn();
                _currentUnitMoving++;
                MovePerformed?.Invoke();
            }
        }


        private bool TryAttackIfInRange(Unit unit)
        {
            var closestUnit = GetOppositeUnitInAttackRange(unit);
            if (closestUnit)
            {
                unit.Attack(closestUnit);
                return true;
            }
            return false;
        }

        private bool TryMoveTowardsClosestPlayer(Unit unit)
        {
            Unit closestUnit = GetClosestUnit(unit, unit.GetOppositeFraction());
            Tile tile = null;
            if(closestUnit)
                tile = GridManager.GetClosestFreeTileTowardsDestination(unit.CurrentTile, closestUnit.CurrentTile, unit.Statistics.MoveRange);

            if (tile != null && tile != unit.CurrentTile)
            {
                unit.MoveToTile(tile);
                return true;
            }
            else
            {
                unit.SkipTurn();
                return false;
            }
        }
        public bool HasAnyMoves()
        {
            if (_currentUnitMoving > units.Count)
                return false;
            for (var i = 0; i < units.Count; i++)
                if (units[i].CanPerformAttack() || units[i].CanPerformMove())
                    return true;
            return false;
        }
        
                
        private void CheckForGameWin(Unit unit)
        {
            if(unit.Fraction == Fraction.ENEMY)
                if(units.Count == 0)
                    Observer.OnGameWin?.Invoke();
        }

        public void EndMyTurn()
        {
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                unit.ResetTurn();
            }

            Observer.OnNextMoveRequested -= MoveOneUnit;
        }

        public string GetName()
        {
            return "Enemy computer";
        }
    }
}
