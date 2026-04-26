using UnityEngine;


public class WarriorAttack : CharacterAttack
{

    [Header("Warrior special info")]
    [SerializeField] private bool _enemyDetected;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRadius;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float _stunDuration = 1f;
    [SerializeField] private int _stunProbabilityInPercent;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleAnimations();
        HandleCollision();
        HandleAttack();
        HandleMovement();
    }

    //Warrior special methods
    private void HandleCollision()
    {
        _enemyDetected = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, _whatIsTarget);
    }

    private void HandleAnimations()
    {
        _character.CharAnimator._anim.SetFloat("xVelocity", _rb.linearVelocity.x);
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _whatIsTarget);
        foreach (Collider2D enemy in enemyColliders)
        {
            Character entityTarget = enemy.GetComponent<Character>();
            entityTarget.TakeDamage(_character.Stats._currentDamage);
            if (_random.Next(1, 101) <= _stunProbabilityInPercent)
            {
                entityTarget.Effects.SetStun(_stunDuration);
            }
        }
    }

    protected void HandleMovement()
    {
        if (canMove)
            _rb.linearVelocity = new Vector2(_character._facingDirection * _moveSpeed, _rb.linearVelocity.y);
        else
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }

    private void HandleAttack()
    {
        if (_character._isDead) return;

        if (_character.canAttack && _enemyDetected)
        {
            _character.CharAnimator._anim.SetBool("Attack", true);
        }
        else
        {
            _character.CharAnimator._anim.SetBool("Attack", false);
            if (_character.Effects._isStunned)
                canMove = false;
        }
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
