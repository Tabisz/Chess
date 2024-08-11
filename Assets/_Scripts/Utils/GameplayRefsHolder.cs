using _Scripts.AI;
using _Scripts.Turns;
using _Scripts.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Utils
{
    public class GameplayRefsHolder : MonoBehaviour, ICustomInitializer,ICustomUpdater
    {
        [SerializeField] private TurnController turnController;
        public TurnController TurnController => turnController;

        [SerializeField] private InGameUIController inGameUIController;
        public InGameUIController InGameUIController => inGameUIController;

        [SerializeField] private Player player;
        public Player Player => player;

        [SerializeField] private GridManager gridManager;
        public GridManager GridManager => gridManager;

        [SerializeField] private UnitsFactory unitsFactory;
        public UnitsFactory UnitsFactory => unitsFactory;
    
        [SerializeField] private PlayerUnitsController playerUnitsController;
        public PlayerUnitsController PlayerUnitsController => playerUnitsController;

        [SerializeField] private SpawningTurnController spawningTurnController;
        public SpawningTurnController SpawningTurnController => spawningTurnController;
    
        [SerializeField] private AiUnitsController aiUnitsController;
        public AiUnitsController AiUnitsController => aiUnitsController;

        public Observer Observer;
        public void Init()
        {
            Observer = new Observer();
            player.Init();
            gridManager.Init();
            playerUnitsController.Init();
            aiUnitsController.Init();
            unitsFactory.Init();
            turnController.Init();
            inGameUIController.Init();
            spawningTurnController.Init();
        }

        public void CustomUpdate()
        {
            turnController.CustomUpdate();
            player.CustomUpdate();
        }

        public void CustomFixedUpdate()
        {
        }
    
        public void Deinit()
        {
            turnController.Deinit();
            gridManager.Deinit();
            playerUnitsController.Deinit();
        }
    }
}
