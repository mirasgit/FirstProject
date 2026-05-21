using FirstProject.CharacterEffect;
using UnityEngine;

public class Fireball : Projectile
{
    WeaknessEffect weakness;
    public void SetEffect(float duration, float coefficient)
    {
        weakness = new WeaknessEffect(duration, coefficient);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Character target))
        {
            target.TakeDamage(_damage);
            if (weakness != null)
            {
            target.ApplyEffect(weakness);
            }
            Destroy(gameObject);
        }
    }
}
