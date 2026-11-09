using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Infrastructures.AssetManagement
{
    [Serializable]
    public sealed class ComponentReference<TComponent> : AssetReferenceGameObject
        where TComponent : MonoBehaviour
    {
        public ComponentReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj) =>
            obj is GameObject gameObject && gameObject.GetComponent<TComponent>() != null;

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            return gameObject != null && gameObject.GetComponent<TComponent>() != null;
#else
            return false;
#endif
        }
    }
}
