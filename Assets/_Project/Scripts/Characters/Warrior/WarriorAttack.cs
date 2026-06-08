using UnityEngine;
using FirstProject.CharacterEffect;

namespace FirstProject.Characters.Attack
{
    public class WarriorAttack : CharacterAttack
    {
        [Header("Warrior special info")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _stunDuration = 1f;
        [SerializeField] private int _stunProbabilityInPercent;

        private Rigidbody2D _rb;
        private StunEffect _stun;

        private bool _canMove = true;
        private bool _enemyDetected;
        private bool _isAttacking;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _stun = new StunEffect(_stunDuration);
        }

        protected override void Update()
        {
            HandleCollision();
            HandleAttack();
            HandleAnimations();
            HandleMovement();
        }

        public void EnableMovement(bool enable)
        {
            _canMove = enable;
        }

        private void HandleCollision()
        {
            _enemyDetected = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _whatIsTarget);
        }

        private void HandleAnimations()
        {
            _charAnimator.SetVelocity(_rb.linearVelocity.x);
        }

        public void DamageTargets()
        {
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _whatIsTarget);
            foreach (Collider2D enemy in enemyColliders)
            {
                Character entityTarget = enemy.GetComponent<Character>();
                entityTarget.TakeDamage(_stats.CurrentDamage);
                int chance = Random.Range(0, 100);
                if (chance <= _stunProbabilityInPercent)
                {
                    entityTarget.ApplyEffect(_stun);
                }
            }
        }

        private void HandleMovement()
        {
            if (_canMove)
            {
                _rb.linearVelocity = new Vector2(_facing.FacingDirection * _moveSpeed, _rb.linearVelocity.y);
            }
            else
            {
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            }
        }

        private void SetAttacking(bool enable)
        {
            if (_isAttacking == enable)
            {
                return;
            }
            _isAttacking = enable;
            _charAnimator.ToggleAttack(enable);
        }

        protected override void HandleAttack()
        {
            if (_death.IsDead)
            {
                return;
            }

            bool shouldAttack = CanAttack() && _enemyDetected;

            SetAttacking(shouldAttack);
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