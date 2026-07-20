using FirstProject.Characters;

namespace FirstProject.CharacterEffect
{
    public class CharacterEffectContext 
    {
        private readonly CharacterStats _stats;
        private readonly CharacterAnimator _animator;
        private readonly IDamageable _damageable;
        public CharacterEffectContext(CharacterStats stats, CharacterAnimator animator, IDamageable model)
        {
            _stats = stats;
            _animator = animator;
            _damageable = model;
        }

        public void TakeDamage(float damage)
        {
            _damageable.TakeDamage(damage);
        }

        public float GetCurrentDamage()
        {
            return _stats.CurrentDamage;
        }

        public void AddDamageModifier(float modifier)
        {
            _stats.AddDamageModifier(modifier);
        }

        public void RemoveDamageModifier(float modifier)
        {
            _stats.RemoveDamageModifier(modifier);
        }

        public void EnablePlayStun(bool enable)
        {
            if (enable)
            {
                _animator.PlayStun();
            }
            else
            {
                _animator.StopPlayStun();
            }
        }
    }
}