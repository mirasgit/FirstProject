using UnityEngine;
using FirstProject.Projectiles;
using Unity.VisualScripting;
using Zenject;

namespace FirstProject.Characters.Attack
{
    public class ArcherAttack : CharacterAttack
    {
        [Header("Ranged special info")]
        [SerializeField] private Arrow _projectile;
        [SerializeField] private float _poisonDuration = 3f;
        [SerializeField] private float _poisonInterval = 2f;
        [SerializeField] private float _poisonTickDamage = 2f;
        [SerializeField] private int _poisonProbabilityInPercent;
        [Inject] private IInstantiator _instantiator;

        public void SpawnProjectile()
        {
            if (_projectile == null || _attackPoint == null)
            {
                return;
            }

            Arrow newProjectile = _instantiator.InstantiatePrefabForComponent<Arrow>(_projectile, _attackPoint.position, _attackPoint.rotation, null);
            newProjectile.SetFacingDirection(_facing.FacingDirection);
            newProjectile.SetDamage(_stats.CurrentDamage);
            newProjectile.Initialize();

            if (Random.value <= _poisonProbabilityInPercent / TO_PERCENT_MULTIPLIER)
            {
                newProjectile.SetEffect(_poisonDuration, _poisonInterval, _poisonTickDamage);
            }
        }
    }
}
