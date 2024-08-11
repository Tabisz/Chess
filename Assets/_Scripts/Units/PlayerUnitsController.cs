using System;

namespace _Scripts.Units
{
    public class PlayerUnitsController : UnitsController, ITurnProvider
    {
        private Unit _currentlySelectedUnit;
    
        public event Action MovePerformed;

        public override void Init()
        {
            base.Init();

            Observer.OnEmptyTileSelected += OnEmptyTileSelected;
            Observer.OnEmptyTileSecondarySelected += OnEmptyTileSecondarySelected;

            Observer.OnUnitSelected += OnUnitSelected;
            Observer.OnUnitSecondarySelected += OnUnitSecondarySelected;
            Observer.OnUnitDied += CheckForGameEnd;
        
            TurnController.PlayerRegisterTurnProvider(this);
        }
    
        private void OnUnitSelected(Unit unit)
        {
            _currentlySelectedUnit = unit.Fraction == Fraction.PLAYER ? unit : null;
        }

        private void OnUnitSecondarySelected(Unit unit)
        {
            if(TurnController.CurrentTurnProvider.GetName() != GetName())
                return;
        
            if(_currentlySelectedUnit!= null)
                if (TryAttackUnit(_currentlySelectedUnit, unit))
                {
                   // _currentlySelectedUnit = null;
                    MovePerformed?.Invoke();
                }
        }

        private void OnEmptyTileSelected(Tile tile)
        {
            _currentlySelectedUnit = null;
        }

        private void OnEmptyTileSecondarySelected(Tile tile)
        {
            if(TurnController.CurrentTurnProvider.GetName() != GetName())
                return;
        
            if (_currentlySelectedUnit != null)
                if (TryMoveUnit(_currentlySelectedUnit, tile))
                {
                    //_currentlySelectedUnit = null;
                    MovePerformed?.Invoke();                
                }
        }

        private void OnMovePerformed()
        {
            if (_currentlySelectedUnit)
            {
                GridManager.ShowRangesForUnit(_currentlySelectedUnit);
                
                if ( !_currentlySelectedUnit.CanPerformMove() && (!_currentlySelectedUnit.CanPerformAttack() || !GetOppositeUnitInAttackRange(_currentlySelectedUnit)))
                    _currentlySelectedUnit.SetSleeping();
            }
            else
                GridManager.RefreshGrid();
        }
    
        private bool TryMoveUnit(Unit unit, Tile tile)
        {
            if ( unit.CanPerformMove() && unit.Fraction ==Fraction.PLAYER && GridManager.IsTileInRange(unit.CurrentTile, tile,
                    unit.Statistics.MoveRange))
            {
                unit.MoveToTile(tile, OnMovePerformed);
                return true;
            }
            return false;
        }

        private bool TryAttackUnit(Unit sourceUnit, Unit destinationUnit)
        {
            if (sourceUnit.CanPerformAttack() && sourceUnit != destinationUnit &&
                sourceUnit.Fraction != destinationUnit.Fraction &&
                GridManager.IsTileInRange(sourceUnit.CurrentTile,
                    destinationUnit.CurrentTile,
                    sourceUnit.Statistics.AttackRange))
            {
                sourceUnit.Attack(destinationUnit, OnMovePerformed);
                return true;
            }
            return false;
        }

        public void StartMyTurn()
        {
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                unit.ResetTurn();
            }
        }

        public bool HasAnyMoves()
        {
            //player may only end turn by clicking button

            foreach (var unit in units)
            {
                if ( unit.CanPerformMove() || (unit.CanPerformAttack() && GetOppositeUnitInAttackRange(unit)))
                    return true;
            }
            return false;
        }

        public void EndMyTurn()
        {
            _currentlySelectedUnit = null;
            Observer.OnEmptyTileSelected?.Invoke(null);
        }

        public string GetName()
        {
            return "Player";
        }

        private void CheckForGameEnd(Unit unit)
        {
            if(unit.Fraction == Fraction.PLAYER)
                if(units.Count == 0)
                    Observer.OnGameLost?.Invoke();
        }
    
        public void Deinit()
        {
            Observer.OnEmptyTileSelected -= OnEmptyTileSelected;
            Observer.OnEmptyTileSecondarySelected -= OnEmptyTileSecondarySelected;
        
            Observer.OnUnitSelected -= OnUnitSelected;
            Observer.OnUnitSecondarySelected -= OnUnitSecondarySelected;
            Observer.OnUnitDied -= CheckForGameEnd;

        }
    }
}
