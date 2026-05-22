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
        newProjectile.SetFacingDirection(_character.FacingDirection());
        newProjectile.SetDamage(_character.Stats.CurrentDamage);
        newProjectile.Initialize(_projectileRegistry);
        if (_random.Next(1, 101) <= _poisonProbabilityInPercent)
        {
            newProjectile.SetEffect(_poisonDuration, _poisonInterval, _poisonTickDamage);
        }
    }
    private void HandleAttack()
    {
        if (_character.BattleStarted)
        {
            if (_character.CanAttack() && !_character.IsDead)
            {
                _character.CharAnimator._anim.SetTrigger("Attack");
            }

        }
    }
}
