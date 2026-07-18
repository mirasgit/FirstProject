using UnityEngine;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CharacterHitBox : MonoBehaviour
    {
        public Character Character { get; private set; }

        private void Awake()
        {
            Character = GetComponentInParent<Character>();
        }
    }
}
