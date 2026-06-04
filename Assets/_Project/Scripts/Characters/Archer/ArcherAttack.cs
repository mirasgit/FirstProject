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
            Arrow newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
            newProjectile.SetFacingDirection(_character.FacingDirection());
            newProjectile.SetDamage(_character.GetCurrentDamage());
            newProjectile.Initialize(_projectileRegistry);
            if (_random.Next(1, 101) <= _poisonProbabilityInPercent)
            {
                newProjectile.SetEffect(_poisonDuration, _poisonInterval, _poisonTickDamage);
            }
        }
    }
}
