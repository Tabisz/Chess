using System.Collections.Generic;

namespace _Scripts.Utils
{
    public class InputLocker 
    {

        private List<string> inputLockers;

        private bool _isLocked;
        public bool IsLocked => _isLocked;

        public void Init()
        {
            inputLockers = new List<string>();
        }


        public void AddInputLocker(string locker)
        {
            inputLockers.Add(locker);
            ReadjustLockedState();
        }

        public void RemoveInputLocker(string locker)
        {
            inputLockers.Remove(locker);
            ReadjustLockedState();
        }
    
        public void ReadjustLockedState()
        {
            _isLocked = inputLockers.Count > 0;

        }
    }
}
