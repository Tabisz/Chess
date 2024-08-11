using _Scripts.Utils;
using UnityEngine;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private Camera gameCamera;
        public Camera GameCamera => gameCamera;
        public StateMachine<State> GameSm { get; private set; }

        [SerializeField] private SceneLoader sceneLoader;
        public SceneLoader SceneLoader => sceneLoader;

        [SerializeField]private GlobalStatistics globalStatistics;
        public GlobalStatistics GlobalStatistics => globalStatistics;
    
    
        [HideInInspector]
        public GameplayRefsHolder gameplayRefsHolder;
    
        private void Awake() 
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);   
                Destroy(this);
                return;
            }
            else
                Instance = this;
            DontDestroyOnLoad(gameObject);
        
            SceneLoader.Init();
        
        
        
            GameSm = new StateMachine<State>();
            GameSm.Init();

#if UNITY_EDITOR
            if(sceneLoader.CurrentScene.MyEnum == SceneEnum.MENU)
                GameSm.ChangeState(new MenuState());
            else
                GameSm.ChangeState(new GameplayState());
#else
                GameSm.ChangeState(new MenuState());
#endif
        }

        private void Update()
        {
            GameSm.CustomUpdate();
        }

        private void FixedUpdate()
        {
            GameSm.CustomFixedUpdate();
        }


    }
}
