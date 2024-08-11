using System;
using UnityEngine;

namespace _Scripts.AI
{
    public class SpawningTurnController : MonoBehaviour, ITurnProvider
    {
        public event Action MovePerformed;

        private bool _getReadyPhase;

        [SerializeField] private GameObject dropZoneMarker;

        private DropZoneMarker[] _dropZoneMarkerPool;
        public void Init()
        {
            var enemyCount = GameController.Instance.GlobalStatistics.GetDifficultyPackage(
                GameController.Instance.RuntimeDataHolder.CurrentDifficultySetting).EnemyCount;
        
            GameController.Instance.GameplayRefsHolder.TurnController.SpawningRegisterTurnProvider(this);
            _getReadyPhase = true;

            _dropZoneMarkerPool = CreateDropZoneMarkersPool(enemyCount);

        }

        private DropZoneMarker[] CreateDropZoneMarkersPool(int poolCount)
        {
            DropZoneMarker[] pool = new DropZoneMarker[poolCount];

            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(dropZoneMarker, transform.position, Quaternion.identity, transform).GetComponent<DropZoneMarker>();
                pool[i].Deinit();
            }

            return pool;
        }
    
    
        public void StartMyTurn()
        {
            if (_getReadyPhase)
            {
                ActivateMarkers();
            }
            else
            {
                TrySpawnNewUnits();
                DeactivateMarkers();
            }
            _getReadyPhase = !_getReadyPhase;
            MovePerformed?.Invoke();
        }
    
        private void ActivateMarkers()
        {
            var grid = GameController.Instance.GameplayRefsHolder.GridManager;
            for (int i = 0; i < _dropZoneMarkerPool.Length; i++)
            {
                var tile = grid.GetRandomFreeTile();
                _dropZoneMarkerPool[i].transform.position = grid.GetPositionOfTile(tile);
                _dropZoneMarkerPool[i].Init(tile);
            }
        }

        private void DeactivateMarkers()
        {
            for (int i = 0; i < _dropZoneMarkerPool.Length; i++)
            {
                _dropZoneMarkerPool[i].Deinit();
            }
        }

        private void TrySpawnNewUnits()
        {
            for (int i = 0; i < _dropZoneMarkerPool.Length; i++)
            {
                var tile = _dropZoneMarkerPool[i].GetTile();
            
                if(tile.CurrentTileOccipier != null)
                    tile.CurrentTileOccipier.OnMyTileKillCommand();
            
                GameController.Instance.GameplayRefsHolder.UnitsFactory.SpawnEnemy(tile, true);
            }
        }

        public bool HasAnyMoves()
        {
            //spawning turn has only one move witch is performed in start turn
            return false;
        }

        public void EndMyTurn()
        {
        }

        public string GetName()
        {
            return "Spawning";
        }
    }
}
