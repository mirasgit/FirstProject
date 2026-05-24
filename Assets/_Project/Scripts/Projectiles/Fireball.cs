using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters;

namespace FirstProject.Projectiles
{
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
}