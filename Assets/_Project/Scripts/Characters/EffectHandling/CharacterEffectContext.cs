using FirstProject.Characters;
using System;


namespace FirstProject.CharacterEffect
{
    public class CharacterEffectContext 
    {
        private readonly CharacterStats _stats;
        private readonly CharacterAnimator _animator;

        private readonly Action<float> _takeDamage;
        
        public CharacterEffectContext(CharacterStats stats, CharacterAnimator animator, Action<float> takeDamage)
        {
            _stats = stats;
            _animator = animator;
            _takeDamage = takeDamage;
        }

        public void TakeDamage(float damage)
        {
            _takeDamage?.Invoke(damage);
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