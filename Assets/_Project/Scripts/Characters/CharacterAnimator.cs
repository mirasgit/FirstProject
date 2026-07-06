using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;
        private readonly int _dieHash = Animator.StringToHash("Die");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _attackSpeedHash = Animator.StringToHash("AttackSpeed");
        private readonly int _stunHash = Animator.StringToHash("Stun");
        private readonly int _velocityHash = Animator.StringToHash("xVelocity");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            _animator.SetTrigger(_dieHash);
        }

        public void PlayAttack()
        {
            _animator.SetTrigger(_attackHash);
        }
        public void SetAttackSpeed(float speed)
        {
            _animator.SetFloat(_attackSpeedHash, speed);
        }

        public void PlayStun()
        {
            _animator.SetBool(_stunHash, true);
        }

        public void StopPlayStun()
        {
            _animator.SetBool(_stunHash, false);
        }

        public void SetVelocity(float parameter)
        {
            _animator.SetFloat(_velocityHash, parameter);
        }
    }
}