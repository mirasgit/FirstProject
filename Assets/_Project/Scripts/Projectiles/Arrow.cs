using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.Projectiles
{
    public class Arrow : Projectile
    {
        private PoisonEffect poison;
        public void SetEffect(float duration, float interval, float tickDamage)
        {
            poison = new PoisonEffect(duration, interval, tickDamage);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character target))
            {
                target.TakeDamage(_damage);
                if (poison != null)
                {
                    target.ApplyEffect(poison);
                }
                Destroy(gameObject);
            }
        }
    }
}