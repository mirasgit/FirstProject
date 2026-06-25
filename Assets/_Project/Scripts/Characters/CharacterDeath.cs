using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterDeath : MonoBehaviour
    {
        private const string DEAD_CHARACTER_LAYER = "DeadCharacter";

        private CharacterAnimator _charAnimator;
        public bool IsDead { get; private set; } = false;


        private void Awake()
        {
            _charAnimator = GetComponent<CharacterAnimator>();
        }

        public bool Die()
        {
            if (IsDead)
            {
                return false;
            }

            IsDead = true;
            gameObject.layer = LayerMask.NameToLayer(DEAD_CHARACTER_LAYER);
            _charAnimator.PlayDeath();

            return true;
        }
    }
}