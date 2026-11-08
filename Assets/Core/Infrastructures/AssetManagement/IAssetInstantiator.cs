using UnityEngine;

namespace Core.Infrastructures.AssetManagement
{
    public interface IAssetInstantiator
    {
        TComponent Instantiate<TComponent>(TComponent prefab, Transform parent, Vector3 pos, Vector3 scale)
            where TComponent : MonoBehaviour;

        void Destroy(Object obj);
    }
}
