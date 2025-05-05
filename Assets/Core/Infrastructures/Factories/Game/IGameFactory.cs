using Core.Logic.Bullet;
using Core.Logic.Enemy;
using Core.Logic.Gun;
using Core.Logic.Level;
using Core.Logic.Player;
using UnityEngine;

namespace Core.Infrastructures.Factories.Game
{
    public interface IGameFactory : IRemoveObject
    {
        PlayerComponent CreatePlayer(Vector2 pos, PlayerConfig config);

        LevelComponent CreateLevel();

        TEnemy CreateEnemy<TEnemy>(EnemyConfig config, Vector3 position, int idBattlePlace)
            where TEnemy : EnemyComponent;

        TGun CreateGun<TGun>(GunConfig config, Transform parent, Vector3 position, Vector3 rotation)
            where TGun : GunComponent;

        TBullet CreateBullet<TBullet>(BulletConfig config, Vector3 position)
            where TBullet : BulletComponent;
    }
}