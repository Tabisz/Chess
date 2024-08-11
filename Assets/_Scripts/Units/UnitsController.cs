using System.Collections.Generic;
using _Scripts.Turns;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Units
{
    public class UnitsController : MonoBehaviour
    {
        protected List<Unit> _units;
        public List<Unit> Units => _units;
    
        protected TurnController _turnController;
        protected Observer _observer;
        protected GridManager _gridManager;

        public virtual void Init()
        {
            _units = new List<Unit>();
            _turnController = GameController.Instance.GameplayRefsHolder.TurnController;
            _observer = GameController.Instance.GameplayRefsHolder.Observer;
            _gridManager = GameController.Instance.GameplayRefsHolder.GridManager;
        }
    
        public void RegisterToUnitsController(Unit unit)
        {
            _units.Add(unit);
        }
        public void UnRegisterToUnitsController(Unit unit)
        {
            _units.Remove(unit);
        }

    }
}
