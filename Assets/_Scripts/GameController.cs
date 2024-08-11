using _Scripts.Utils;
using UnityEngine;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private Camera _camera;
        public Camera Camera => _camera;
        public StateMachine<State> GameSm { get; private set; }

        [SerializeField] private SceneLoader _sceneLoader;
        public SceneLoader SceneLoader => _sceneLoader;

        public GlobalStatistics GlobalStatistics => _globalStatistics;
        [SerializeField]private GlobalStatistics _globalStatistics;
    
    
        [HideInInspector]
        public GameplayRefsHolder GameplayRefsHolder;
    
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
            if(_sceneLoader.CurrentScene.MyEnum == SceneEnum.MENU)
                GameSm.ChangeState(new MenuState());
            else
                GameSm.ChangeState(new GameplayState());
#else
                GameSM.ChangeState(new MenuState());
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
