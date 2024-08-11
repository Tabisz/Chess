using System;
using _Scripts.Units;

namespace _Scripts.Utils
{
    public class Observer
    {
        public Action<Tile> OnEmptyTileSelected;
        public Action<Tile> OnEmptyTileSecondarySelected;

        public Action<Unit> OnUnitSelected;
        public Action<Unit> OnUnitSecondarySelected;

        public Action<Unit> OnDamageReceived;

        public Action<Unit> OnUnitDied;
        public Action OnGameLost;

        public Action OnNextMoveRequested;
        
    }
}
