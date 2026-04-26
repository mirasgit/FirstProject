using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Character : MonoBehaviour
{

    [field: SerializeField] public CharacterStats Stats { get; private set; }
    [field: SerializeField] public CharacterEffects Effects { get; private set; }
    [field: SerializeField] public CharacterAnimator CharAnimator { get; private set; }
    [field:SerializeField] public CharacterAttack Attack { get; private set; }

    [SerializeField] private Transform _visualRoot;
    [field: SerializeField] public int _facingDirection { get; private set; } = 1;

    [SerializeField] public bool FacingRight = true;

    public CharacterUI _characterUI { get; private set;}
    public bool _battleStarted { get; private set; } = false;

    public bool canMove { get; private set; }
    public bool _isDead { get; private set; } = false;
    public bool canAttack { get; private set; } = true;
    public ProjectileRegistry _projectileRegistry {  get; private set; }

    public event Action<float, float> HealthChanged;

    protected void Awake()
    {
        _characterUI = GetComponentInChildren<CharacterUI>();
        Stats = GetComponent<CharacterStats>();
        Effects = GetComponent<CharacterEffects>();
        CharAnimator = GetComponent<CharacterAnimator>();
        Attack = GetComponent<CharacterAttack>();
    }

    private void Update()
    {
        HandleDeath();
    }

    public ProjectileRegistry ProjectileRegistry => _projectileRegistry;
    public void InitializeProjectileRegistry(ProjectileRegistry projectileRegistry)
    {
        _projectileRegistry = projectileRegistry;
    }

    public void TakeDamage(float damage)
    {
        if (!_isDead)
        {
            Stats.TakeDamage(damage);
            HealthChanged?.Invoke(Stats._currentHealth, Stats._maxHealth);  
            if (_characterUI != null)
                _characterUI.ShowDamage(damage);
        }
    }

    public void EnableAttack(bool enable)
    {
        canAttack = enable;
    }
    public void EnableMovement(bool enable) // only for Warrior as it requires canMove
    {
        canMove = enable;
    }
    public void StartBattle()
    {
        _battleStarted = true;

    }
    public void ResetCharacterState()
    {
        Stats.ResetCharacterStats();
        Effects.ResetEffects();
        _battleStarted = false;
    }

    private void HandleDeath()
    {
        if (Stats._currentHealth <= 0)
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

    private void Die()
    {
        _battleStarted = false;
        gameObject.layer = LayerMask.NameToLayer("DeadCharacter");
        CharAnimator._anim.SetTrigger("Die");
        Effects.ResetEffects();
        canAttack = false;
        canMove = false;
    }


    public void SetFacingRight(bool facingRight)
    {
        FacingRight = facingRight;
        _facingDirection = facingRight ? 1 : -1;

        _visualRoot.localRotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);

        Debug.Log($"{name}: FacingRight={FacingRight}, Direction={_facingDirection}");
    }

}