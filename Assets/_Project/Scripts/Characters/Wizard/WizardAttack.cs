using UnityEngine;
using FirstProject.Projectiles;

namespace FirstProject.Characters.Attack
{
    public class WizardAttack : CharacterAttack
    {
        [Header("Wizard special info")]
        [SerializeField] private Fireball _projectile;
        [SerializeField] private float _weaknessDuration = 4f;
        [SerializeField] private float _weaknessCoefficient = 0.8f;
        [SerializeField] private int _weaknessProbabilityInPercent;

        public void SpawnProjectile()
        {
            if (_projectile == null || _attackPoint == null)
            {
                return;
            }

            Fireball newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
            newProjectile.SetFacingDirection(_facing.FacingDirection);
            newProjectile.SetDamage(_stats.CurrentDamage);
            newProjectile.Initialize(_projectileRegistry);
            int chance = Random.Range(0, 100);

            if (chance <= _weaknessProbabilityInPercent)
            {
                newProjectile.SetEffect(_weaknessDuration, _weaknessCoefficient);
            }
        }
    }
}