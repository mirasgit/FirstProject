using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterDeath : MonoBehaviour
    {
        [SerializeField] private LayerMask _deadLayer;
        private CharacterAnimator _charAnimator;
        private int _deadLayerIndex;

        public bool IsDead { get; private set; } = false;
        
        private void Awake()
        {
            _charAnimator = GetComponent<CharacterAnimator>();
            _deadLayerIndex = Mathf.RoundToInt(Mathf.Log(_deadLayer, 2));
        }

        public bool Die()
        {
            if (IsDead)
            {
                return false;
            }

            IsDead = true;
            gameObject.layer = _deadLayerIndex;
            _charAnimator.PlayDeath();

            return true;
        }
    }
}