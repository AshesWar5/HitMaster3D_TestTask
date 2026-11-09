using UnityEngine;

namespace Core.Logic.Level
{
    public sealed class RandomLevelSelector : ILevelSelector
    {
        private readonly LevelsConfig _levelsConfig;

        public RandomLevelSelector(LevelsConfig levelsConfig) =>
            _levelsConfig = levelsConfig;

        public LevelData Select() =>
            _levelsConfig.Levels[Random.Range(0, _levelsConfig.Levels.Length)];
    }
}
