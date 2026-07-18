using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterDeath : MonoBehaviour
    {
        [SerializeField] private LayerMask _deadLayer;
        private CharacterAnimator _characterAnimator;
        private int _deadLayerIndex;

        public bool IsDead { get; private set; } = false;
        
        private void Awake()
        {
            _characterAnimator = GetComponent<CharacterAnimator>();
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
            _characterAnimator.PlayDeath();

            return true;
        }
    }
}