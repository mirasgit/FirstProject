using UnityEngine;
using FirstProject.Common;

namespace FirstProject.Characters
{
    public class CharacterDeath : MonoBehaviour
    {
        private CharacterAnimator _charAnimator;
        [field: SerializeField] public bool IsDead { get; private set; } = false;

        private void Awake()
        {
            _charAnimator = GetComponent<CharacterAnimator>();
        }

        public void Die()
        {
            if (IsDead)
                return;

            IsDead = true;

            gameObject.layer = LayerMask.NameToLayer(CharacterConstants.DeadCharacterLayer);

            _charAnimator.PlayDeath();
        }
        public void ResetDeath()
        {
            IsDead = false;

            gameObject.layer = LayerMask.NameToLayer(CharacterConstants.CharacterLayer);
        }
    }
}