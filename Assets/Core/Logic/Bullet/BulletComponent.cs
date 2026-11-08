using Core.Logic.Health;
using Core.Logic.Pool;
using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Bullet
{
    public class BulletComponent : Component, IPoolObject
    {
        [SerializeField] private float _maxTimeLife = 5;
        
        private float _damage;
        private IPoolReturn _pool;
        private float _currentTimeLife;

        private float _speed;

        public override void Updating()
        {
            if(!IsActiveComponent)
                return;

            if (_currentTimeLife >= _maxTimeLife)
            {
                ReturnToPool();
                return;
            }

            _currentTimeLife += Time.deltaTime;
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        public void SetDamageValue(float value) =>
            _damage = value;

        public void Move(Vector3 direction, float speed)
        {
            _speed = speed;
            transform.LookAt(direction);
        }

        public void AssignPool(IPoolReturn pool) => _pool = pool;

        public void Enable() => Show();

        public void Disable() => Hide();

        public void ReturnToPool()
        {
            if(!IsActiveComponent || _pool is null)
                return;

            _currentTimeLife = 0;
            _pool.ReturnToPool(this);
        }

        public void ReleaseFromPool() => _pool = null;

        private void OnTriggerEnter(Collider other)
        {
            if(!IsActiveComponent)
                return;

            if (other.TryGetComponent(out IHealth health))
            {
                health.SetDamage(_damage);
            }

            ReturnToPool();
        }
    }
}