using UnityEngine;

public class WizardAttack : CharacterAttack
{

    [Header("Wizard special info")]
    [SerializeField] private Fireball _projectile;
    [SerializeField] private float _weaknessDuration = 4f;
    [SerializeField] private float _weaknessCoefficient = 0.8f;
    [SerializeField] private int _weaknessProbabilityInPercent;


    private void Update()
    {
        HandleAttack();
    }
    public void SpawnProjectile()
    {
        Fireball newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
        newProjectile.SetFacingDirection(_character.FacingDirection());
        newProjectile.SetDamage(_character.Stats.CurrentDamage);
        newProjectile.Initialize(_projectileRegistry);
        if (_random.Next(1, 101) <= _weaknessProbabilityInPercent)
        {
            newProjectile.SetEffect(_weaknessDuration, _weaknessCoefficient);
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
