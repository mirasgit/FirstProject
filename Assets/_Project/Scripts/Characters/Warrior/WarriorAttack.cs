using UnityEngine;
using FirstProject.CharacterEffect;


namespace FirstProject.Characters.Attack
{
    public class WarriorAttack : CharacterAttack
    {
        private const int MAX_TARGETS = 1;

        [Header("Warrior special info")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _stunDuration;
        [SerializeField] private int _stunProbabilityInPercent;

        private readonly Collider2D[] _enemyColliders = new Collider2D[MAX_TARGETS];
        private Rigidbody2D _rb;

        private int _hitCount;
        private bool _enemyDetected;
        private float _currentMoveSpeed;
        private ContactFilter2D _targetFilter;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _targetFilter = new ContactFilter2D();
            _targetFilter.SetLayerMask(_whatIsTarget);
            _targetFilter.useLayerMask = true;
            _targetFilter.useTriggers = true;
        }

        protected override void Update()
        {
            DetectEnemies();
            HandleAttack();
            SetVelocity();
        }

        private void DetectEnemies()
        {
            _hitCount = GetTargets();
            _enemyDetected = _hitCount > 0;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private int GetTargets()
        {
            return Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _targetFilter, _enemyColliders);
        }

        private void SetVelocity()
        {
            _charAnimator.SetVelocity(_currentMoveSpeed);
        }

        public void DamageTargets()
        {
            if (_hitCount <= 0) 
            {
                return;
            }
            for (int i = 0; i < _hitCount; i++)
            {
                Collider2D enemy = _enemyColliders[i];

                if (!enemy.TryGetComponent(out CharacterHitBox hitBox))
                {
                    continue;
                }

                Character target = hitBox.Character;

                if (target == null || target.IsDead)
                {
                    continue;
                }

                target.TakeDamage(_stats.CurrentDamage);


                if (Random.value <= _stunProbabilityInPercent / TO_PERCENT_MULTIPLIER)
                {
                    target.ApplyEffect(new StunEffect(_stunDuration));
                }
            }
        }

        private void HandleMovement()
        {
            if (_death.IsDead || _effects.HasEffect(EffectType.Stun) || _enemyDetected)
            {
                _currentMoveSpeed = 0f;
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                return;
            }
            _currentMoveSpeed = _moveSpeed;
            _rb.linearVelocity = new Vector2(_facing.FacingDirection * _moveSpeed, _rb.linearVelocity.y);

        }

        protected override void HandleAttack()
        {
            if (!_enemyDetected)
            {
                return;
            }

            base.HandleAttack();
        }

        private void OnDrawGizmos()
        {
            if (_attackPoint == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }
    }
}