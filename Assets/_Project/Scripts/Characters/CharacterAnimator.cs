using UnityEngine;

namespace FirstProject.Characters
{

    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _anim;
        private readonly int DieHash = Animator.StringToHash("Die");
        private readonly int AttackHash = Animator.StringToHash("Attack");
        private readonly int StunHash = Animator.StringToHash("Stun");
        private readonly int VelocityHash = Animator.StringToHash("xVelocity");

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            _anim.SetTrigger(DieHash);
        }
        public void PlayAttack()
        {
            _anim.SetTrigger(AttackHash);
        }
        public void ToggleAttack(bool enable)
        {
            _anim.SetBool(AttackHash, enable);
        }
        public void PlayStun()
        {
            _anim.SetBool(StunHash, true);
        }
        public void StopPlayStun()
        {
            _anim.SetBool(StunHash, false);
        }
        public void SetVelocity(float parameter)
        {
            _anim.SetFloat(VelocityHash, parameter);
        }
    }
}