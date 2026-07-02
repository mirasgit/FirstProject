using UnityEngine;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CharacterHitBox : MonoBehaviour
    {
        private Character _character;

        public Character Character =>_character;

        private void Awake()
        {
            _character = GetComponentInParent<Character>();
        }
    }
}
