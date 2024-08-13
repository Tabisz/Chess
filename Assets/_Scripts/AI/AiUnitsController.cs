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
            if (true)
            {
                MovePerformed += MoveOneUnit;
                MoveOneUnit();
            }
            else
            {
                Observer.OnNextMoveRequested += MoveOneUnit;
            }
        }

        private void MoveOneUnit()
        {
            if (units.Count <= _currentUnitMoving)
            {
                EndMyTurn();
                return;                
            }

            var unit = units[_currentUnitMoving];

            if (unit.CanPerformAttack() && TryAttackIfInRange(unit)) //try to attack
                return;
            
            if (unit.CanPerformMove() && TryMoveTowardsClosestPlayer(unit)) //try to move
                return;
            
            // do nothing
            unit.SkipTurn();
            _currentUnitMoving++;
            MovePerformed?.Invoke();
        }


        private bool TryAttackIfInRange(Unit unit)
        {
            var closestUnit = GetOppositeUnitInAttackRange(unit);
            if (closestUnit)
            {
                unit.Attack(closestUnit, OnAttackPerformed);
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
                unit.MoveToTile(tile, OnMovePerformed);
                return true;
            }
            return false;
            
        }

        public void OnAttackPerformed()
        {
            _currentUnitMoving++;
            MovePerformed?.Invoke();
        }

        public void OnMovePerformed()
        {
            MovePerformed?.Invoke();
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
            MovePerformed -= MoveOneUnit;
        }

        public string GetName()
        {
            return "Enemy computer";
        }
    }
}
