using UnityEngine;
using Zenject;
using FirstProject.CharacterEffect;
using FirstProject.Configs;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        private float _moveSpeed;

        private CharacterFacing _facing;
        private CharacterAnimator _characterAnimator;
        private CharacterDeath _death;
        private CharacterEffects _effects;
        private RemoteConfigService _configService;

        private Rigidbody2D _rb;
        private float _currentMoveSpeed;
        private bool _isAllowedToMove;

        [Inject]
        public void Construct(CharacterFacing facing, CharacterAnimator animator, CharacterDeath death, CharacterEffects effects, RemoteConfigService configService, CharacterIdentity identity)
        {
            _facing = facing;
            _characterAnimator = animator;
            _death = death;
            _effects = effects;
            _configService = configService;

            var config = _configService.GetCharacterConfig(identity.MyClass);
            _moveSpeed = config.MoveSpeed;
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