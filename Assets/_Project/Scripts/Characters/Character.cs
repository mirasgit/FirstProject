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
    [field: SerializeField] public int FacingDirection { get; private set; } = 1;

    [field:SerializeField] public bool FacingRight = true;

    public CharacterUI CharacterUI { get; private set;}
    public bool BattleStarted { get; private set; } = false;

    public bool CanMove { get; private set; }
    [field: SerializeField] public bool IsDead { get; private set; } = false;
    public bool CanAttack { get; private set; } = true;
    public ProjectileRegistry ProjectileRegistry {  get; private set; }

    public event Action<float, float> HealthChanged;

    public event Action<float> DamageTaken;

    protected void Awake()
    {
        CharacterUI = GetComponentInChildren<CharacterUI>();
        Stats = GetComponent<CharacterStats>();
        Effects = GetComponent<CharacterEffects>();
        CharAnimator = GetComponent<CharacterAnimator>();
        Attack = GetComponent<CharacterAttack>();
    }

    private void Update()
    {
        HandleDeath();
    }

    public void InitializeProjectileRegistry(ProjectileRegistry projectileRegistry)
    {
        ProjectileRegistry = projectileRegistry;
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead)
        {
            Stats.TakeDamage(damage);
            HealthChanged?.Invoke(Stats.CurrentHealth, Stats.MaxHealth);
            DamageTaken?.Invoke(damage);
        }
    }

    public void EnableAttack(bool enable)
    {
        CanAttack = enable;
    }
    public void EnableMovement(bool enable) // only for Warrior as it requires canMove
    {
        CanMove = enable;
    }
    public void StartBattle()
    {
        BattleStarted = true;

    }
    public void ResetCharacterState()
    {
        Stats.ResetCharacterStats();
        Effects.ResetEffects();
        BattleStarted = false;
    }

    private void HandleDeath()
    {
        if (Stats.CurrentHealth > 0 || IsDead)
        {
            return;
        }
        IsDead = true;
        Die();
    }

    private void Die()
    {
        BattleStarted = false;
        gameObject.layer = LayerMask.NameToLayer("DeadCharacter");
        CharAnimator._anim.SetTrigger("Die");
        Effects.ResetEffects();
        CanAttack = false;
        CanMove = false;
    }


    public void SetFacingRight(bool facingRight)
    {
        FacingRight = facingRight;
        FacingDirection = facingRight ? 1 : -1;

        _visualRoot.localRotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);

    }

}