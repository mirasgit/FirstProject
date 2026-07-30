using UnityEngine;
using Zenject;
using FirstProject.Projectiles;
using FirstProject.CharacterEffect;
namespace FirstProject.Characters
{
    public class RangedAttack : CharacterAttack
    {
        [SerializeField] protected Projectile _projectilePrefab;

        [SerializeField] protected EffectConfig _effectConfig;

        [SerializeField, Range(0, 100)] protected int _applyProbabilityPercent;

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
            CharacterApplicableEffect effectToApply = null;

            if (_effectConfig != null && Random.value <= _applyProbabilityPercent / TO_PERCENT_MULTIPLIER)
            {
                effectToApply = _effectConfig.CreateEffect();
            }

            newProjectile.SetEffect(effectToApply);
        }
    }
}