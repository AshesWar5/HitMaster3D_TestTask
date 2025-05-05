using Zenject;
using UnityEngine;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private readonly DiContainer _diContainer;

        public AssetProvider(DiContainer diContainer) =>
            _diContainer = diContainer;

        public TType Instantiate<TType>(string path) where TType : MonoBehaviour
        {
            return Instantiate<TType>(path, null, Vector3.zero, Vector3.one);
        }
        
        public TType Instantiate<TType>(string path, Vector3 pos) where TType : MonoBehaviour
        {
            return Instantiate<TType>(path, null, pos, Vector3.one);
        }
        
        public TType Instantiate<TType>(string path, Vector3 pos, Vector3 scale) where TType : MonoBehaviour
        {
            return Instantiate<TType>(path, null, pos, scale);
        }

        public TType Instantiate<TType>(string path, Transform parent, Vector3 pos, Vector3 scale)
            where TType : MonoBehaviour
        {
            var asset = Resources.Load<TType>(path);
            return Instantiate(asset, parent, pos, scale);
        }
        
        public TType Instantiate<TType>(TType obj, Transform parent, Vector3 pos, Vector3 scale)
            where TType : MonoBehaviour
        {
            var instance = _diContainer.InstantiatePrefab(obj, Vector3.zero, Quaternion.identity,
                parent).GetComponent<TType>();
            instance.transform.localPosition = pos;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = scale;
            return instance;
        }

        public void Destroy(Object obj)
        {
            if (obj is not null)
            {
                Object.Destroy(obj);
            }
        }
    }
}