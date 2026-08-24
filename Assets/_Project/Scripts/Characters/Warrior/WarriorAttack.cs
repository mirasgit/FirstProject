using UnityEngine;
using Zenject;

namespace FirstProject.Characters   
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class WarriorAttack : CharacterAttack
    {
        private const int MAX_TARGETS = 1;

        [Header("Warrior special info")]
        [SerializeField] private float _attackRadius;

        private readonly Collider2D[] _enemyColliders = new Collider2D[MAX_TARGETS];

        private int _hitCount;
        private bool _enemyDetected;
        private ContactFilter2D _targetFilter;
        private CharacterMovement _movement;

        private void Awake()
        {
            _targetFilter = new ContactFilter2D();
            _targetFilter.SetLayerMask(_whatIsTarget);
            _targetFilter.useLayerMask = true;
            _targetFilter.useTriggers = true;
        }

        [Inject]
        public void ConstructWarrior(CharacterMovement movement)
        {
            _movement = movement;
        }

        private void DetectEnemies()
        {
            if (!CanAttack())  
            {
                _enemyDetected = false;
                return;
            }

            _hitCount = GetTargets();
            _enemyDetected = _hitCount > 0;
        }

        private void FixedUpdate()
        {
            DetectEnemies();

            if (!CanAttack())
            {
                _movement.AllowToMove(false);
            }
            else
            {
                _movement.AllowToMove(!_enemyDetected);
            }
        }

        private int GetTargets()
        {
            return Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _targetFilter, _enemyColliders);
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

                target.TakeDamage(_stats.CurrentDamage, _stats.MyClass);

                target.ApplyEffect(TryGetEffect());

            }
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