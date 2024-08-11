using UnityEngine;
namespace _Scripts.AI
{
    public class DropZoneMarker : MonoBehaviour
    {
        private Tile _currentTile;

        public void Init(Tile currentTile)
        {
            gameObject.SetActive(true);
            _currentTile = currentTile;
        }

        public Tile GetTile()
        {
            return _currentTile;
        }

        public void Deinit()
        {
            _currentTile = null;
            gameObject.SetActive(false);
        }
    
    }
}
