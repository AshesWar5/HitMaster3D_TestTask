using UnityEngine;

namespace Core.Logic.Level
{
    [CreateAssetMenu(fileName = "Levels Config", menuName = "Level/List", order = 0)]
    public sealed class LevelsConfig : ScriptableObject
    {
        [SerializeField] private LevelComponent[] _levels;
        
        public LevelComponent[] Levels => _levels;
    }
}