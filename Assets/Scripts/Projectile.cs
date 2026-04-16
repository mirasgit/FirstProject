using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int _facingDirection = 1;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _damage;

    protected Rigidbody2D rb;
    protected float _effectDuration;
    Coroutine DestryHandlerCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    private void Update()
    {
        HandleMovement();
        HandleDestroy();
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
        rb.linearVelocity = new Vector2(_facingDirection * _moveSpeed, rb.linearVelocity.y);
    } 

    private void HandleDestroy()
    {
        if(DestryHandlerCoroutine != null)
            StopCoroutine(DestryHandlerCoroutine);
        StartCoroutine(DestroyHandlerCo());
    }
    private IEnumerator DestroyHandlerCo()
    {
        yield return new WaitForSeconds(5);
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