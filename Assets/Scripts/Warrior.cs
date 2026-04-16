using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;

public class Warrior : Character
{
    [Header("Warrior special info")]
    [SerializeField] private bool _enemyDetected;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected bool canMove = true;
    [SerializeField] private float _stunDuration = 1f;
    [SerializeField] private int _stunProbabilityInPercent;
    protected override void Update()
    {
        HandleAnimations();
        HandleFlip();
        HandleCollision();
        HandleAttack();
        HandleMovement();
        HandleDeath();
        HandleEffects();
    }

    //Warrior special methods
    private void HandleCollision()
    {
        _enemyDetected = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _whatIsTarget);
    }

    private void HandleAnimations()
    {
        _anim.SetFloat("xVelocity", _rb.linearVelocity.x);
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _whatIsTarget);
        foreach (Collider2D enemy in enemyColliders)
        {
            Character entityTarget = enemy.GetComponent<Character>();
            entityTarget.TakeDamage(_currentDamage);
            if (random.Next(1,101) <= _stunProbabilityInPercent)
            {
            entityTarget.SetEffect(_stunDuration);
            }
        }
    }

    protected void HandleMovement()
    {
        if (canMove)
            _rb.linearVelocity = new Vector2(_facingDirection * _moveSpeed, _rb.linearVelocity.y);
        else
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }

    protected override void HandleAttack()
    {
        if(_isDead) return;

        if (canAttack && _enemyDetected)
        {
            _anim.SetBool("Attack", true);
        }
        else
        {
            _anim.SetBool("Attack", false);
            if(_isStunned) 
                canMove = false;
        }
    }
    protected override IEnumerator StunCo()
    {
        Debug.Log("Warrior StunCo started");

        canAttack = false;
        canMove = false;
        _anim.SetBool("Stun", true);
        _characterUI?.ShowEffect("Оглушение");

        yield return new WaitForSeconds(_effectDuration);

        _characterUI?.HideEffect();
        _anim.SetBool("Stun", false);
        canAttack = true;
        canMove = true;
        _isStunned = false;
        _isStunApplied = false;
    }
    protected override void Die()
    {
            _battleStarted = false;
            canAttack = false;
            canMove = false;
            gameObject.layer = LayerMask.NameToLayer("DeadCharacter");
            _anim.SetTrigger("Die");
    }
    public void EnableMovement(bool enable)
    {
        canMove = enable;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
