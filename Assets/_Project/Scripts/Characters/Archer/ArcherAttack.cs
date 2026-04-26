using UnityEngine;

public class ArcherAttack : CharacterAttack
{
    [Header("Ranged special info")]
    [SerializeField] private Arrow _projectile;
    [SerializeField] private float _poisonDuration = 3f;
    [SerializeField] private float _poisonInterval = 2f;
    [SerializeField] private float _poisonTickDamage = 2f;
    [SerializeField] private int _poisonProbabilityInPercent;
   
    private void Update()
    {
        HandleAttack();
    }

    public void SpawnProjectile()
    {
        Arrow newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
        newProjectile.SetFacingDirection(_character._facingDirection);
        newProjectile.SetDamage(_character.Stats._currentDamage);
        newProjectile.Initialize(_character.ProjectileRegistry);
        if (_random.Next(1, 101) <= _poisonProbabilityInPercent)
        {
            newProjectile.SetEffect(_poisonDuration, _poisonInterval, _poisonTickDamage);
        }
    }
    private void HandleAttack()
    {
        if (_character._battleStarted)
        {
            if (_character.canAttack && !_character._isDead)
            {
                _character.CharAnimator._anim.SetTrigger("Attack");
            }

        }
    }
}
