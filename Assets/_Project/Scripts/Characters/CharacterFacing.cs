using UnityEngine;
using FirstProject.Common;

namespace FirstProject.Characters
{
    public class CharacterFacing : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        [field: SerializeField] public bool FacingRight = true;
        [field: SerializeField] public int FacingDirection { get; private set; }

        public void SetFacingRight(bool facingRight)
        {
            FacingRight = facingRight;
            FacingDirection = facingRight ? CharacterConstants.RightDirection : CharacterConstants.LeftDirection;

            float rotationY = facingRight ? CharacterConstants.RightRotationY : CharacterConstants.LeftRotationY;

            _visualRoot.localRotation = Quaternion.Euler(
                    CharacterConstants.ZeroRotation,
                    rotationY,
                    CharacterConstants.ZeroRotation);
        }
    }
}