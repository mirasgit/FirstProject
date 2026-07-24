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

        protected IInstantiator _instantiator;

        [Inject]
        public void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public virtual void SpawnProjectile()
        {
            if (_projectilePrefab == null || _attackPoint == null)
            {
                return;
            }

            Projectile newProjectile = _instantiator.InstantiatePrefabForComponent<Projectile>(_projectilePrefab, _attackPoint.position, _attackPoint.rotation, null);
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