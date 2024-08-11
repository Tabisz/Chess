using UnityEngine;

namespace _Scripts.Utils
{
    public class MainMenuRefsHolder : MonoBehaviour, ICustomUpdater,ICustomInitializer
    {
        [SerializeField]
        private MainMenuUIController mainMenuUIController;
        public MainMenuUIController MainMenuUIController => mainMenuUIController;

        public void Init()
        {
            mainMenuUIController.Init();
        }
    
        public void CustomUpdate()
        {
        }

        public void CustomFixedUpdate()
        {
        }

        public void Deinit()
        {
            mainMenuUIController.Deinit();
        }
    }
}
