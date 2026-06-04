using UnityEngine;
using FirstProject.Characters;
using FirstProject.CharacterEffect;

namespace FirstProject.Projectiles
{
    public class Arrow : Projectile
    {
        private PoisonEffect _poison;
        public void SetEffect(float duration, float interval, float tickDamage)
        {
            _poison = new PoisonEffect(duration, interval, tickDamage);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character target))
            {
                target.TakeDamage(_damage);
                if (_poison != null)
                {
                    target.ApplyEffect(_poison);
                }
                Destroy(gameObject);
            }
        }
    }
}