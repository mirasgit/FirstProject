using UnityEngine;
using FirstProject.Projectiles;
using Zenject;

namespace FirstProject.Characters.Attack
{
    public class WizardAttack : CharacterAttack
    {
        [Header("Wizard special info")]
        [SerializeField] private Fireball _projectile;
        [SerializeField] private float _weaknessDuration = 4f;
        [SerializeField] private float _weaknessCoefficient = 0.8f;
        [SerializeField] private int _weaknessProbabilityInPercent;
        [Inject] private IInstantiator _instantiator;

        public void SpawnProjectile()
        {
            if (_projectile == null || _attackPoint == null)
            {
                return;
            }

            Fireball newProjectile = _instantiator.InstantiatePrefabForComponent<Fireball>(_projectile, _attackPoint.position, _attackPoint.rotation, null);
            newProjectile.SetFacingDirection(_facing.FacingDirection);
            newProjectile.SetDamage(_stats.CurrentDamage);
            newProjectile.Initialize();

            if (Random.value <= _weaknessProbabilityInPercent / TO_PERCENT_MULTIPLIER)
            {
                newProjectile.SetEffect(_weaknessDuration, _weaknessCoefficient);
            }
        }
    }
}