using UnityEngine;
using Zenject;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class AssetInstantiator : IAssetInstantiator
    {
        private readonly DiContainer _diContainer;

        public AssetInstantiator(DiContainer diContainer) =>
            _diContainer = diContainer;

        public TComponent Instantiate<TComponent>(TComponent prefab, Transform parent, Vector3 pos, Vector3 scale)
            where TComponent : MonoBehaviour
        {
            var instance = _diContainer.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity,
                parent).GetComponent<TComponent>();
            instance.transform.localPosition = pos;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = scale;

            return instance;
        }

        public void Destroy(Object obj)
        {
            if(obj == null)
                return;

            Object.Destroy(obj);
        }
    }
}
