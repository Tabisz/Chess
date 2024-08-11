using UnityEngine;

namespace _Scripts.Units
{
    [CreateAssetMenu(fileName = "Unit Statistics", menuName = "Data/Unit Statistics")]
    public class UnitStatistics : ScriptableObject
    {
        [SerializeField]
        private int maxHp;
        public int MaxHp => maxHp;
    
        [SerializeField]
        private int _dmg;
        public int Dmg => _dmg;
    
        [SerializeField]
        private int _attackRange;
        public int AttackRange => _attackRange;
    
        [SerializeField]
        private int _moveRange;
        public int MoveRange => _moveRange;


        [SerializeField]
        private float _attackSpeed;
        public float AttackSpeed => _attackSpeed;
        [SerializeField]
        private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;
    
    }
}
