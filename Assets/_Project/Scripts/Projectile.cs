using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected int _facingDirection = 1;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _damage;
    [SerializeField] protected int _secondsToDestroy = 4;

    protected ProjectileRegistry _projectileRegistry;
    protected Rigidbody2D _rb;
    protected float _effectDuration;
    protected Coroutine _destroyHandlerCoroutine;
    protected WaitForSeconds _waitDestroy;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        _waitDestroy = new WaitForSeconds(_secondsToDestroy);
    }

    private void Update()
    {
        HandleMovement();
        HandleDestroy();
    }
    public void Initialize(ProjectileRegistry _projectileRegistry)
    {
        _projectileRegistry.Register(this);
    }

    protected virtual void OnDestroy()
    {
        if (_projectileRegistry != null)
        {
            _projectileRegistry.Unregister(this);
        }
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

    private void HandleDestroy()
    {
        if(_destroyHandlerCoroutine != null)
            StopCoroutine(_destroyHandlerCoroutine);
        StartCoroutine(DestroyHandlerCo());
    }
    private IEnumerator DestroyHandlerCo()
    {
        yield return _waitDestroy;
        Destroy(this.gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Character target))
        {
            target.TakeDamage(_damage);
            Destroy(this.gameObject);
        }
    }
}