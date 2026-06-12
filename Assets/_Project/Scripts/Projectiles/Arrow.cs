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
            if (collision.gameObject.TryGetComponent(out CharacterHitBox target))
            {
                target.Character.TakeDamage(_damage);
                if (_poison != null)
                {
                    target.Character.ApplyEffect(_poison);
                }
                Destroy(gameObject);
            }
        }
    }
}