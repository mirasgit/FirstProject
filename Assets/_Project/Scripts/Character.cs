using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Character : MonoBehaviour
{
    [Header("Health info")]
    [field:SerializeField] public float _maxHealth { get; private set; } = 100;
    [field:SerializeField] public float _currentHealth { get; private set; } 
    [SerializeField] protected float _maxDamage = 10;
    [SerializeField] protected float _currentDamage;


    [Header("Attack info")]
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected LayerMask _whatIsTarget;
    [SerializeField] protected int _facingDirection = 1;
    [SerializeField] public bool FacingRight = true;

    protected CharacterUI _characterUI;
    protected bool _battleStarted = false;
    protected Coroutine _effectCoroutine;
    protected Rigidbody2D _rb;
    protected Collider2D _col;
    protected Animator _anim;
    public bool _isDead { get; private set; } = false;
    protected bool canAttack = true;

    [SerializeField] protected bool _isWeak;
    [SerializeField] protected bool _isWeaknessApplied;
    [SerializeField] protected bool _isPoisoned;
    [SerializeField] protected bool _isPoisonApplied;
    [SerializeField] protected bool _isStunned;
    [SerializeField] protected bool _isStunApplied;

    protected ProjectileRegistry _projectileRegistry;
    protected float _effectDuration;
    protected float _weaknessCoefficient;
    protected float _poisonInterval;
    protected float _poisonTickDamage;
    protected Random _random = new Random();

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        _currentHealth = _maxHealth;
        _currentDamage = _maxDamage;
        _characterUI = GetComponentInChildren<CharacterUI>();
    }

    protected virtual void Update()
    {
        HandleFlip();
        HandleDeath();
        HandleEffects();
        HandleAttack();
    }

    public void InitializeProjectileRegistry(ProjectileRegistry projectileRegistry)
    {
        _projectileRegistry = projectileRegistry;
    }

    public void StartBattle()
    {
        _battleStarted = true;
    }
    public virtual void ResetCharacterState()
    {
        _currentHealth = _maxHealth;
        _isDead = false;
        _battleStarted = false;
    }

    protected void HandleDeath()
    {
        if (_currentHealth <= 0)
        {
            if (_isDead)
            {
                return;
            }
            else
            {
                _isDead = true;
                Die();
            }
        }
    }

    protected virtual void HandleAttack()
    {
        if (_battleStarted) { 

        if (canAttack && !_isDead)
            _anim.SetTrigger("Attack");

    }   }
    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        FacingRight = !FacingRight;
        _facingDirection *= -1;
    }
    protected void HandleFlip()
    {
        if (!FacingRight)
        {
            Flip();
        }
    }

    protected virtual void Die()
    {
        _battleStarted = false;
        gameObject.layer = LayerMask.NameToLayer("DeadCharacter");
        _anim.SetTrigger("Die");
    }
    public void TakeDamage(float damage)
    {
        if(_isDead) { return; }

        _currentHealth -= damage;

        if(_characterUI != null)
            _characterUI.ShowDamage(damage);


    }
    public void SetEffect(float duration) //Stun
    {
        _isStunned = true;
        _effectDuration = duration;
    }
    public void SetEffect(float duration, float coefficient) //weakness
    {
        _isWeak = true;
        _effectDuration = duration;
        _weaknessCoefficient = coefficient;
    }
    
    public void SetEffect(float duration, float interval, float tickDamage) //poison
    {
        if(_isPoisoned) { return; }
        _isPoisoned = true;
        _effectDuration = duration;
        _poisonInterval = interval;
        _poisonTickDamage = tickDamage;
    }
    protected virtual IEnumerator StunCo()
    {
        canAttack = false;
        _anim.SetBool("Stun", true);
        _characterUI.ShowEffect("ќглушение");
        
        yield return new WaitForSeconds(_effectDuration);
        _characterUI.HideEffect();
        _anim.SetBool("Stun", false);
        canAttack = true;
        _isStunned = false;
        _isStunApplied = false;
    }
    protected IEnumerator WeaknessCo()
    {
        float reducedDmg = _maxDamage *  (1 - _weaknessCoefficient);
        _currentDamage = reducedDmg;
        _characterUI.ShowEffect("—лабость");
        yield return new WaitForSeconds(_effectDuration);
        _characterUI.HideEffect();
        _currentDamage = _maxDamage;
        _isWeak = false;
        _isWeaknessApplied = false;
    }
    protected IEnumerator PoisonCo()
    {
        float elapsed = 0f;
        _characterUI.ShowEffect("яд");
        while (elapsed < _effectDuration)
        {
            Debug.Log("PoisonTick");
            TakeDamage(_poisonTickDamage);
            yield return new WaitForSeconds(_poisonInterval);
            elapsed += _poisonInterval;
        }
        _characterUI.HideEffect();
        _isPoisoned = false;
        _isPoisonApplied = false;
    }
    protected void HandleEffects()
    {
        if (_isStunned && !_isDead && _battleStarted && !_isStunApplied)
        {
            PlayEffectApplication(StunCo());
            _isStunApplied = true;
        }
        if (_isWeak && !_isDead && _battleStarted && !_isWeaknessApplied)
        {
            PlayEffectApplication(WeaknessCo());
            _isWeaknessApplied = true;
        }
        if (_isPoisoned && !_isDead && _battleStarted && !_isPoisonApplied)
        {
            PlayEffectApplication(PoisonCo());
            _isPoisonApplied = true;
        }
    }

    protected void PlayEffectApplication(IEnumerator effect)
    {
        if (_effectCoroutine != null)
            StopCoroutine(_effectCoroutine);

        _effectCoroutine = StartCoroutine(effect);
    }
}