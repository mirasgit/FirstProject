using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterDeath : MonoBehaviour
    {
        private CharacterAnimator _charAnimator;
        [field: SerializeField] public bool IsDead { get; private set; } = false;

        private const string DEAD_CHARACTER_LAYER = "DeadCharacter";
        private const string CHARACTER_LAYER = "Character";
        private void Awake()
        {
            _charAnimator = GetComponent<CharacterAnimator>();
        }

        public void Die()
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;

            gameObject.layer = LayerMask.NameToLayer(DEAD_CHARACTER_LAYER);

            _charAnimator.PlayDeath();
        }
        public void ResetDeath()
        {
            IsDead = false;

            gameObject.layer = LayerMask.NameToLayer(CHARACTER_LAYER);
        }
    }
}