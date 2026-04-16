using UnityEngine;

public class Ranger : Character
{
    [Header("Ranged special info")]
    [SerializeField] private Arrow _projectile; //Ranger
    [SerializeField] private float _effectDuration = 3f;
    [SerializeField] private float _poisonInterval = 2f;
    [SerializeField] private float _poisonTickDamage = 2f;
    [SerializeField] private int _poisonProbabilityInPercent;

    public void SpawnProjectile() //ranger
    {
        Arrow newProjectile = Instantiate(_projectile, _attackPoint.position, _attackPoint.rotation);
        newProjectile.SetFacingDirection(_facingDirection);
        newProjectile.SetDamage(_currentDamage);
        if (random.Next(1, 101) <= _poisonProbabilityInPercent)
        {
            newProjectile.SetEffect(_effectDuration, _poisonInterval, _poisonTickDamage);
        }
    }
}
