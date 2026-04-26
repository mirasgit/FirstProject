using UnityEngine;

public class Fireball : Projectile
{
    private float _weaknessCoefficient;
    public void SetEffect(float duration, float coefficient)
    {
        _effectDuration = duration;
        _weaknessCoefficient = coefficient;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Character target))
        {
            target.TakeDamage(_damage);
            target.Effects.SetWeakness(_effectDuration, _weaknessCoefficient);
            Destroy(gameObject);
        }
    }
}
