using UnityEngine;
using Zenject;
using FirstProject.Projectiles;
namespace FirstProject.Characters
{
    public class RangedAttack : CharacterAttack
    {
        [SerializeField] protected Projectile _projectilePrefab;

        protected ProjectileFactory _factory;

        [Inject]
        public void Construct(ProjectileFactory factory)
        {
            _factory = factory;
        }

        public virtual void SpawnProjectile()
        {
            if (_projectilePrefab == null || _attackPoint == null)
            {
                return;
            }

            var config = _configService.GetCharacterConfig(_identity.MyClass);

            Projectile newProjectile = _factory.Create(_projectilePrefab, _attackPoint.position, _attackPoint.rotation);

            newProjectile.Initialize(_identity.MyClass, _stats.CurrentDamage, _facing.FacingDirection, TryGetEffect(), config.Projectile);
        }
    }
}