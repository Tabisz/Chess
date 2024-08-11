using _Scripts.Utils;
using UnityEngine;

//Here pooling system should be implemented but i'm short on time :P

namespace _Scripts.Units
{
    public class UnitsFactory : MonoBehaviour
    {
        public GameObject UnitPrefab;
        private GridManager _gridManager;

    
        public void Init()
        {
            _gridManager = GameController.Instance.GameplayRefsHolder.GridManager;
            SpawnPlayerStartingUnits();
            SpawnEnemyStartingUnits();

        }

        private void SpawnPlayerStartingUnits()// might move starting player army to global config
        {
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.TANK, false);
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.ARCHER, false);
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.PLAYER, UnitType.KNIGHT,false);
        }

        private void SpawnEnemyStartingUnits()
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetCurrentDifficultyPackage();
            for (int i = 0; i < difficultySetting.EnemyCount; i++)
            {
                SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.ENEMY, difficultySetting.EnemyUnitType,false);
            }
        }

        public void SpawnEnemyAtRandom()
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetCurrentDifficultyPackage();
            SpawnUnit(_gridManager.GetRandomFreeTile(), Fraction.ENEMY, difficultySetting.EnemyUnitType,false);
        }

        public void SpawnEnemy(Tile tile, bool disabledForNextTurn)
        {
            var difficultySetting = GameController.Instance.GlobalStatistics.GetCurrentDifficultyPackage();
            SpawnUnit(tile, Fraction.ENEMY, difficultySetting.EnemyUnitType, disabledForNextTurn);
        }
    
    

        private void SpawnUnit(Tile tile, Fraction fraction, UnitType type, bool disabledForNextTurn)
        {
            var _unit =  Instantiate(UnitPrefab, tile.transform.position, Quaternion.identity).GetComponent<Unit>();
            var _package = GameController.Instance.GlobalStatistics.GetUnitPackage(type);
        
            if(fraction == Fraction.ENEMY)
                _unit.Init(GameController.Instance.GameplayRefsHolder.AiUnitsController, fraction, _package.statistics, _package.graphic, disabledForNextTurn);
            else
                _unit.Init(GameController.Instance.GameplayRefsHolder.PlayerUnitsController, fraction, _package.statistics, _package.graphic, disabledForNextTurn);

            _unit.SpawnAtTile(tile);

        }

        public void Deinit()
        {
        
        }
    }
}
