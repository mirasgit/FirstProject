using UnityEngine;

namespace FirstProject.Characters
{

    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            _anim.SetTrigger("Die");
        }
        public void PlayAttack()
        {
            _anim.SetTrigger("Attack");
        }
        public void ToggleAttack(bool enable)
        {
            _anim.SetBool("Attack", enable);
        }
        public void PlayStun()
        {
            _anim.SetBool("Stun", true);
        }
        public void StopPlayStun()
        {
            _anim.SetBool("Stun", false);
        }
        public void SetVelocity(float parameter)
        {
            _anim.SetFloat("xVelocity", parameter);
        }
    }
}