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

        protected override void ApplyEffect(Character target)
        {
            if (_poison == null)
            {
                return;
            }

            target.ApplyEffect(_poison);
        }
    }
}