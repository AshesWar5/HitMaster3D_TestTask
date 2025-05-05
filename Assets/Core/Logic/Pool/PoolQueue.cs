using System.Collections.Generic;

 namespace Core.Logic.Pool
{
    public sealed class PoolQueue<TObject> : IPoolReturn where TObject : IPoolObject
    {
        private readonly Queue<TObject> _inactiveObjects;
        private readonly List<TObject> _listObjects;

        public IReadOnlyList<TObject> ListObjects => _listObjects;

        public PoolQueue()
        {
            _inactiveObjects = new Queue<TObject>();
            _listObjects = new List<TObject>();
        }

        public void Add(TObject obj) => obj.AssignPool(this);

        public void Clear()
        {
            _inactiveObjects.Clear();
            _listObjects.Clear();
        }

        public void ReturnToPool(IPoolObject obj)
        {
            obj.Disable();
            _inactiveObjects.Enqueue((TObject)obj);
            _listObjects.Add((TObject)obj);
        }

        public bool HasInactiveObjects() => _inactiveObjects.Count > 0;

        public int GetLengthPool() => _inactiveObjects.Count;

        public TObject GetInactiveObject()
        {
            var inactiveObject = _inactiveObjects.Dequeue();
            _listObjects.Remove(inactiveObject);
            inactiveObject.Enable();
            return inactiveObject;
        }
    }
}