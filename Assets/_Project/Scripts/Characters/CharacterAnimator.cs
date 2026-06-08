using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _anim;
        private readonly int _dieHash = Animator.StringToHash("Die");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _stunHash = Animator.StringToHash("Stun");
        private readonly int _velocityHash = Animator.StringToHash("xVelocity");

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            _anim.SetTrigger(_dieHash);
        }

        public void PlayAttack()
        {
            _anim.SetTrigger(_attackHash);
        }

        public void ToggleAttack(bool enable)
        {
            _anim.SetBool(_attackHash, enable);
        }

        public void PlayStun()
        {
            _anim.SetBool(_stunHash, true);
        }

        public void StopPlayStun()
        {
            _anim.SetBool(_stunHash, false);
        }

        public void SetVelocity(float parameter)
        {
            _anim.SetFloat(_velocityHash, parameter);
        }
    }
}