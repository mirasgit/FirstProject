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

            Projectile newProjectile = _factory.Create(_projectilePrefab, _attackPoint.position, _attackPoint.rotation);
            newProjectile.SetFacingDirection(_facing.FacingDirection);
            newProjectile.SetDamage(_stats.CurrentDamage);
            newProjectile.SetEffect(TryGetEffect());
            newProjectile.SetHost(_stats.MyClass);
        }
    }
}