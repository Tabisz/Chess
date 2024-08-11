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
            TurnController = GameController.Instance.gameplayRefsHolder.TurnController;
            Observer = GameController.Instance.gameplayRefsHolder.Observer;
            GridManager = GameController.Instance.gameplayRefsHolder.GridManager;
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
