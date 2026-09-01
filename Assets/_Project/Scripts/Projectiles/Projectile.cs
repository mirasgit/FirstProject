using UnityEngine;
using FirstProject.Characters;
using Zenject;
using FirstProject.CharacterEffect;
using FirstProject.MatchupConfigs;
using FirstProject.Configs;

namespace FirstProject.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private float _moveSpeed = 5f;

        private CharacterClass _attackerClass;
        private ProjectileRegistry _projectileRegistry;
        private Rigidbody2D _rb;
        private CharacterApplicableEffect _effect;

        private float _damage;
        private int _facingDirection = 1;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        [Inject]
        public void Construct(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
            _projectileRegistry.Register(this);
        }

        private void OnDestroy()
        {
            if (_projectileRegistry == null)
            {
                return;
            }
            _projectileRegistry.Unregister(this);
        }

        public void Initialize(
    CharacterClass attackerClass,
    float damage,
    int facingDirection,
    CharacterApplicableEffect effect,
    ProjectileSettings config)
        {
            _attackerClass = attackerClass;
            _damage = damage;
            _facingDirection = facingDirection;
            _effect = effect;

            _moveSpeed = config.MoveSpeed;

            Destroy(gameObject, config.LifeSpanInSeconds);
        }

        private void HandleMovement()
        {
            _rb.linearVelocity = new Vector2(_facingDirection * _moveSpeed, _rb.linearVelocity.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent(out CharacterHitBox hitbox))
            {
                return;
            }

            Character target = hitbox.Character;

            if (target == null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_damage, _attackerClass);
            ApplyEffect(target);
            Destroy(gameObject);
        }

        private void ApplyEffect(Character target)
        {
            target.ApplyEffect(_effect);
        }
    }
}