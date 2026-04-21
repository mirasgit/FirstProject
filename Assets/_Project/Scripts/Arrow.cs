using UnityEngine;

public class Arrow : Projectile
{
    private float _poisonInterval;
    private float _poisonTickDamage;

    public void SetEffect(float duration, float interval, float tickDamage)
    {
        _effectDuration = duration;
        _poisonInterval = interval;
        _poisonTickDamage = tickDamage;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Character target))
        {
            target.TakeDamage(_damage);
            target.SetEffect(_effectDuration, _poisonInterval, _poisonTickDamage);
            Destroy(this.gameObject);
        }
    }
}
