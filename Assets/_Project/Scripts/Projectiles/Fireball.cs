using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters;

namespace FirstProject.Projectiles
{
    public class Fireball : Projectile
    {
        private WeaknessEffect _weakness;

        public void SetEffect(float duration, float coefficient)
        {
            _weakness = new WeaknessEffect(duration, coefficient);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out CharacterHitBox target))
            {
                target.Character.TakeDamage(_damage);
                if (_weakness != null)
                {
                    target.Character.ApplyEffect(_weakness);
                }
                Destroy(gameObject);
            }
        }
    }
}