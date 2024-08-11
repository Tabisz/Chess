using _Scripts.Utils;
using UnityEngine;

namespace _Scripts
{
    public class Player : MonoBehaviour, ICustomInitializer, ICustomUpdater
    {

        public StateMachine<PlayerState> PlayerSM;
    
        public InputLocker PlayerInputLocker { get; private set; }

        public RaycastingManager Raycaster;


        public void Init()
        {
            PlayerInputLocker = new InputLocker();
            PlayerInputLocker.Init();
        
            PlayerSM = new StateMachine<PlayerState>();
            PlayerSM.Init();
            PlayerSM.ChangeState(new PlayerGameplayState());

            Raycaster.Init();
        }

        public void CustomUpdate()
        {
            PlayerSM.CustomUpdate();
        }

        public void CustomFixedUpdate()
        {
        }

        public void Deinit()
        {
        }
    
    }
}
