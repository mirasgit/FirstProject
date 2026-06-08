using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected int _secondsToDestroy = 4;
        [SerializeField] protected float _moveSpeed = 5f;

        private ProjectileRegistry _projectileRegistry;
        private Rigidbody2D _rb;

        protected float _damage;
        protected int _facingDirection = 1;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        public void Initialize(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
            _projectileRegistry.Register(this);

            Destroy(gameObject, _secondsToDestroy);
        }

        protected virtual void OnDestroy()
        {
            if (_projectileRegistry == null)
            {
                return;
            }
            _projectileRegistry.Unregister(this);
        }

        public void SetFacingDirection(int facingDirection)
        {
            _facingDirection = facingDirection;
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        private void HandleMovement()
        {
            _rb.linearVelocity = new Vector2(_facingDirection * _moveSpeed, _rb.linearVelocity.y);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character target))
            {
                target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}