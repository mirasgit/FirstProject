using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterFacing : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;

        private const int RIGHT_DIRECTION = 1;
        private const int LEFT_DIRECTION = -1;
        private const float RIGHT_ROTATION_Y = 0f;
        private const float LEFT_ROTATION_Y = 180f;
        private const float ZERO_ROTATION = 0f;
        public int FacingDirection { get; private set; }

        public void SetFacingRight(bool facingRight)
        {
            FacingDirection = facingRight ? RIGHT_DIRECTION : LEFT_DIRECTION;

            float rotationY = facingRight ? RIGHT_ROTATION_Y : LEFT_ROTATION_Y;

            _visualRoot.localRotation = Quaternion.Euler(
                    ZERO_ROTATION,
                    rotationY,
                    ZERO_ROTATION);
        }
    }
}