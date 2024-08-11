using System;
using System.Collections.Generic;
using _Scripts.Units;
using UnityEngine;

namespace _Scripts.Utils
{
    [CreateAssetMenu(fileName = "Global Statistics", menuName = "Data/Global Statistics")]
    public class GlobalStatistics : ScriptableObject
    {
        [SerializeField] private List<UnitPackage> UnitsPackages;

        [SerializeField] private List<TilePackage> TilePackages;
        
        [SerializeField]
        private List<DifficultyPackage> DifficultyPackages;

    
        public UnitPackage GetUnitPackage(UnitType type)
        {
            return UnitsPackages.Find(package => package.type == type);
        }

        public Color GetTileColor(TileColor colorName)
        {
            return TilePackages.Find(package => package.colorName == colorName).color;
        }
        
        public DifficultyPackage GetDifficultyPackage(DifficultySetting difficultySetting)
        {
            return DifficultyPackages.Find(x => x.SettingName == difficultySetting);
        }
    }

    public enum UnitType
    {
        ARCHER,
        KNIGHT,
        TANK,
        ENEMY_EASY,
        ENEMY_MEDIUM,
        ENEMY_HARD,
    }

    [Serializable]
    public struct UnitPackage
    {
        public UnitType type;
        public UnitStatistics statistics;
        public Material graphic;
    }

    public enum DifficultySetting
    {
        EASY,
        MEDIUM,
        HARD,
    }

    [Serializable]
    public struct DifficultyPackage
    {
        public DifficultySetting SettingName;
        public int EnemyCount;
        public UnitType EnemyUnitType;
    }
    public enum TileColor
    {
        BASE,
        OFFSET,
        HIGHLIGHT,
        BAD_HIGHLIGHT,
        RANGE_HIGHLIGHT,
    }

    [Serializable]
    public struct TilePackage
    {
        public TileColor colorName;
        public Color color;
    }
}