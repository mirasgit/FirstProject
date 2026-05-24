using UnityEngine;

namespace FirstProject.Characters
{

    public class CharacterAnimator : MonoBehaviour
    {
        public Animator _anim { get; private set; }

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            _anim.SetTrigger("Die");
        }
    }
}