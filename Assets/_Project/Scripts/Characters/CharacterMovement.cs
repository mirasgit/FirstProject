using UnityEngine;
using Zenject;
using FirstProject.CharacterEffect;

namespace FirstProject.Characters
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;

        private CharacterFacing _facing;
        private CharacterAnimator _characterAnimator;
        private CharacterDeath _death;
        private CharacterEffects _effects;

        private Rigidbody2D _rb;
        private float _currentMoveSpeed;
        bool _isAllowedToMove;

        [Inject]
        public void Construct(CharacterFacing facing, CharacterAnimator animator, CharacterDeath death, CharacterEffects effects)
        {
            _facing = facing;
            _characterAnimator = animator;
            _death = death;
            _effects = effects;
        }

        public void HandleMovement()
        {
            if (!CanMove())
            {
                _currentMoveSpeed = 0f;
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                return;
            }
            _currentMoveSpeed = _moveSpeed;
            _rb.linearVelocity = new Vector2(_facing.FacingDirection * _moveSpeed, _rb.linearVelocity.y);

        }

        public void AllowToMove(bool allowed)
        {
            _isAllowedToMove = allowed;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            SetVelocity();
        }

        private bool CanMove()
        {
            return !_death.IsDead && _isAllowedToMove && !_effects.HasEffect(EffectType.Stun);
        }

        private void SetVelocity()
        {
            _characterAnimator.SetVelocity(_currentMoveSpeed);
        }
    }
}