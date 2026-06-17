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

        protected override void ApplyEffect(Character target)
        {
            if (_weakness == null)
            {
                return;
            }

            target.ApplyEffect(_weakness);
        }

    }
}