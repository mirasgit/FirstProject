using UnityEngine;

namespace FirstProject.Characters.Attack
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class WarriorAttack : CharacterAttack
    {
        private const int MAX_TARGETS = 1;

        [Header("Warrior special info")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackRadius;

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
            base.Update();
            SetVelocity();
        }

        private void DetectEnemies()
        {
            if (_death.IsDead || !CanAttack())  
            {
                _enemyDetected = false;
                return;
            }

            _hitCount = GetTargets();
            _enemyDetected = _hitCount > 0;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            DetectEnemies();
        }

        private int GetTargets()
        {
            return Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _targetFilter, _enemyColliders);
        }

        private void SetVelocity()
        {
            _characterAnimator.SetVelocity(_currentMoveSpeed);
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

                target.ApplyEffect(TryGetEffect());

            }
        }

        private void HandleMovement()
        {
            if (!CanAttack() || _enemyDetected)
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