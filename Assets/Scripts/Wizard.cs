using UnityEngine;

public class Wizard : Character
{

    [Header("Wizard special info")]
    [SerializeField] private Fireball _projectile;
    [SerializeField] private float _weaknessDuration = 1f;
    [SerializeField] private float _weaknessCoefficient = 0.8f;
    [SerializeField] private int _weaknessProbabilityInPercent;

    public void SpawnProjectile() 
    {
        Fireball newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
        newProjectile.SetFacingDirection(_facingDirection);
        newProjectile.SetDamage(_currentDamage);
        if (random.Next(1, 101) <= _weaknessProbabilityInPercent)
        {
            newProjectile.SetEffect(_weaknessDuration, _weaknessCoefficient);
        }
    }
}
