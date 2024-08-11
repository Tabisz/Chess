using UnityEngine;
namespace _Scripts.AI
{
    public class DropZoneMarker : MonoBehaviour
    {
        private Tile currentTile;

        public void Init(Tile _currentTile)
        {
            gameObject.SetActive(true);
            this.currentTile = _currentTile;
        }

        public Tile GetTile()
        {
            return currentTile;
        }

        public void Deinit()
        {
            currentTile = null;
            gameObject.SetActive(false);
        }
    
    }
}
