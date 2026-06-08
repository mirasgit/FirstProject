using FirstProject.Characters;
using System;


namespace FirstProject.CharacterEffect
{
    public class CharacterEffectContext 
    {
        public CharacterStats Stats { get; private set; }
        public CharacterAnimator Animator{ get; private set; }

        private readonly Action<float> _takeDamage;
        
        public CharacterEffectContext(CharacterStats stats, CharacterAnimator animator, Action<float> takeDamage)
        {
            Stats = stats;
            Animator = animator;
            _takeDamage = takeDamage;
        }

        public void TakeDamage(float damage)
        {
            _takeDamage?.Invoke(damage);
        }

        public float GetCurrentDamage()
        {
            return Stats.CurrentDamage;
        }

        public void ChangeDamageTo(float damage)
        {
            Stats.ChangeDamage(damage);
        }

        public void EnablePlayStun(bool enable)
        {
            if (enable)
            {
                Animator.PlayStun();
            }
            else
            {
                Animator.StopPlayStun();
            }
        }
    }
}