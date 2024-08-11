using _Scripts.AI;
using _Scripts.Turns;
using _Scripts.Units;
using UnityEngine;

namespace _Scripts.Utils
{
    public class GameplayRefsHolder : MonoBehaviour, ICustomInitializer,ICustomUpdater
    {
        [SerializeField] private TurnController _turnController;
        public TurnController TurnController => _turnController;

        [SerializeField] private InGameUIController inGameUIController;
        public InGameUIController InGameUIController => inGameUIController;

        [SerializeField] private Player _player;
        public Player Player => _player;

        [SerializeField] private GridManager _gridManager;
        public GridManager GridManager => _gridManager;

        [SerializeField] private UnitsFactory _unitsFactory;
        public UnitsFactory UnitsFactory => _unitsFactory;
    
        [SerializeField] private PlayerUnitsController _playerUnitsController;
        public PlayerUnitsController PlayerUnitsController => _playerUnitsController;

        [SerializeField] private SpawningTurnController _spawningTurnController;
        public SpawningTurnController SpawningTurnController => _spawningTurnController;
    
        [SerializeField] private AiUnitsController _aiUnitsController;
        public AiUnitsController AiUnitsController => _aiUnitsController;

        public Observer Observer;
        public void Init()
        {
            Observer = new Observer();
            _player.Init();
            _gridManager.Init();
            _playerUnitsController.Init();
            _aiUnitsController.Init();
            _unitsFactory.Init();
            _turnController.Init();
            inGameUIController.Init();
            _spawningTurnController.Init();


        }

        public void CustomUpdate()
        {
            _turnController.CustomUpdate();
            _player.CustomUpdate();
        }

        public void CustomFixedUpdate()
        {
        }
    
        public void Deinit()
        {
            _turnController.Deinit();
            _gridManager.Deinit();
            _playerUnitsController.Deinit();
        }
    }
}
