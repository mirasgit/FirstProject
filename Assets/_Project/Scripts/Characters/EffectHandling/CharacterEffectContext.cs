using FirstProject.Characters;
using System;


namespace FirstProject.CharacterEffect
{
    public class CharacterEffectContext 
    {
        private readonly CharacterStats _stats;
        private readonly CharacterAnimator _animator;

        private readonly IDamageable _damageable;
        
        public CharacterEffectContext(CharacterStats stats, CharacterAnimator animator)
        {
            _animator = animator;
            _damageable = stats;
            _stats = stats;
        }

        public void TakeDamage(float damage)
        {
            _damageable.TakeDamage(damage);
        }

        public float GetCurrentDamage()
        {
            return _stats.CurrentDamage;
        }

        public void ChangeDamageTo(float damage)
        {
            _stats.ChangeDamage(damage);
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