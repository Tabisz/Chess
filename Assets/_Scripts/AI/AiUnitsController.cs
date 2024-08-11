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
            _turnController.AiRegisterTurnProvider(this);
        }

        public void StartMyTurn()
        {
            _currentUnitMoving = 0;
            _observer.OnNextMoveRequested += MoveOneUnit;
        }

        private void MoveOneUnit()
        {
            var unit = _units[_currentUnitMoving];

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
            else
            {
                unit.SkipTurn();
                _currentUnitMoving++;
                MovePerformed?.Invoke();
            }
        

        }


        private bool TryAttackIfInRange(Unit unit)
        {
            Unit closestUnit = GetClosestUnit(unit);
            var hasUnitInAttackRange = closestUnit!= null &&
                                       _gridManager.IsTileInRange(unit.CurrentTile, closestUnit.CurrentTile, unit.Statistics.AttackRange);
            if (hasUnitInAttackRange)
            {
                unit.Attack(closestUnit);
                return true;
            }
            return false;
        }

        private bool TryMoveTowardsClosestPlayer(Unit unit)
        {
            var closestUnit = GetClosestUnit(unit);
            Tile tile = null;
            if(closestUnit)
                tile = _gridManager.GetClosestFreeTileTowardsDestination(unit.CurrentTile, closestUnit.CurrentTile, unit.Statistics.MoveRange);

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

        private Unit GetClosestUnit(Unit unit)
        {
            int min = int.MaxValue;
            Unit closestUnit = null;
            foreach (var playerUnit in GameController.Instance.GameplayRefsHolder.PlayerUnitsController.Units)
            {
                int distance = _gridManager.GetDistance(unit.CurrentTile, playerUnit.CurrentTile);
                if (distance < min)
                {
                    min = distance;
                    closestUnit = playerUnit;
                }
            }

            return closestUnit;
        }
        public bool HasAnyMoves()
        {
            if (_currentUnitMoving > _units.Count)
                return false;
            for (var i = 0; i < _units.Count; i++)
                if (_units[i].CanPerformAttack() || _units[i].CanPerformMove())
                    return true;
            return false;
        }

        public void EndMyTurn()
        {
            for (var i = 0; i < _units.Count; i++)
            {
                var unit = _units[i];
                unit.ResetTurn();
            }

            _observer.OnNextMoveRequested -= MoveOneUnit;
        }

        public string GetName()
        {
            return "Enemy computer";
        }
    }
}
