using System.Collections.Generic;
using _Scripts.Turns;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Units
{
    public class UnitsController : MonoBehaviour
    {
        protected List<Unit> units;
        public List<Unit> Units => units;
    
        protected TurnController TurnController;
        protected Observer Observer;
        protected GridManager GridManager;

        public virtual void Init()
        {
            units = new List<Unit>();
            TurnController = GameController.Instance.GameplayRefsHolder.TurnController;
            Observer = GameController.Instance.GameplayRefsHolder.Observer;
            GridManager = GameController.Instance.GameplayRefsHolder.GridManager;
        }
        
        protected Unit GetClosestUnit(Unit unit, Fraction FractionToFind)
        {
            int min = int.MaxValue;
            Unit closestUnit = null;
            List<Unit> UnitsCollection;
            if (FractionToFind == Fraction.ENEMY)
                UnitsCollection = GameController.Instance.GameplayRefsHolder.AiUnitsController.Units;
            else
                UnitsCollection = GameController.Instance.GameplayRefsHolder.PlayerUnitsController.Units;
            
            foreach (var otherUnit in UnitsCollection)
            {
                int distance = GridManager.GetDistance(unit.CurrentTile, otherUnit.CurrentTile);
                if (distance < min)
                {
                    min = distance;
                    closestUnit = otherUnit;
                }
            }

            return closestUnit;
        }

        protected Unit GetOppositeUnitInAttackRange(Unit unit)
        {
            
            Unit closestUnit = GetClosestUnit(unit, unit.GetOppositeFraction());
            if (closestUnit != null &&
                GridManager.IsTileInRange(unit.CurrentTile, closestUnit.CurrentTile, unit.Statistics.AttackRange))
                return closestUnit;
            else
                return null;
        }
    
        public void RegisterToUnitsController(Unit unit)
        {
            units.Add(unit);
        }
        public void UnRegisterToUnitsController(Unit unit)
        {
            units.Remove(unit);
        }

    }
}
