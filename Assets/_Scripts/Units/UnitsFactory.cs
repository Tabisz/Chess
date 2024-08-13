using _Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

//Here pooling system should be implemented but i'm short on time :P

namespace _Scripts.Units
{
    public class UnitsFactory : MonoBehaviour
    {
        public GameObject unitPrefab;
        private GridManager _gridManager;

    
        public void Init()
        {
            _gridManager = GameController.Instance.GameplayRefsHolder.GridManager;
            SpawnPlayerStartingUnits();
            SpawnEnemyStartingUnits();

        }

        private void SpawnPlayerStartingUnits()// might move starting player army to global config
        {
           // SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.TANK, false);
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.ARCHER, false);
           // SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.KNIGHT,false);
        }

        private void SpawnEnemyStartingUnits()
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetDifficultyPackage(
                GameController.Instance.RuntimeDataHolder.CurrentDifficultySetting);
            for (int i = 0; i < difficultySetting.EnemyCount; i++)
            {
                SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.ENEMY, difficultySetting.EnemyUnitType,false);
            }
        }

        public void SpawnEnemyAtRandom()
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetDifficultyPackage(
                GameController.Instance.RuntimeDataHolder.CurrentDifficultySetting);
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.ENEMY, difficultySetting.EnemyUnitType,false);
        }

        public void SpawnEnemy(Tile tile, bool disabledForNextTurn)
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetDifficultyPackage(
                GameController.Instance.RuntimeDataHolder.CurrentDifficultySetting);
            SpawnUnit(tile, Fraction.ENEMY, difficultySetting.EnemyUnitType, disabledForNextTurn);
        }
    
    

        private void SpawnUnit(Tile tile, Fraction fraction, UnitType type, bool disabledForNextTurn)
        {
            var unit =  Instantiate(unitPrefab, tile.transform.position, Quaternion.identity).GetComponent<Unit>();
            var package = GameController.Instance.GlobalStatistics.GetUnitPackage(type);
        
            if(fraction == Fraction.ENEMY)
                unit.Init(GameController.Instance.GameplayRefsHolder.AiUnitsController, fraction, package.statistics, package.graphic, disabledForNextTurn);
            else
                unit.Init(GameController.Instance.GameplayRefsHolder.PlayerUnitsController, fraction, package.statistics, package.graphic, disabledForNextTurn);

            unit.SpawnAtTile(tile);

        }

        public void Deinit()
        {
        
        }
    }
}
