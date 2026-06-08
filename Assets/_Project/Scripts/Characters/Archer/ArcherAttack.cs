using UnityEngine;
using FirstProject.Projectiles;

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

        public void SpawnProjectile()
        {
            if (_projectile == null || _attackPoint == null)
            {
                return;
            }

            Arrow newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
            newProjectile.SetFacingDirection(_facing.FacingDirection);
            newProjectile.SetDamage(_stats.CurrentDamage);
            newProjectile.Initialize(_projectileRegistry);
            int chance = Random.Range(0, 100);

            if (chance <= _poisonProbabilityInPercent)
            {
                newProjectile.SetEffect(_poisonDuration, _poisonInterval, _poisonTickDamage);
            }
        }
    }
}
