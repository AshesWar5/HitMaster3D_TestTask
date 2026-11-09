using System;
using Core.Infrastructures.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Logic.Level
{
    [Serializable]
    public sealed class LevelData
    {
        [SerializeField] private ComponentReference<LevelComponent> _prefab;
        [SerializeField] private AssetLabelReference _label;

        public ComponentReference<LevelComponent> Prefab => _prefab;
        public string Label => _label.labelString;
    }
}
