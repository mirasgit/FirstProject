using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters.Attack;   
using FirstProject.Projectiles;
using FirstProject.UI;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(CharacterEffects))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(CharacterDeath))]
    [RequireComponent(typeof(CharacterFacing))]
    [RequireComponent(typeof(CharacterAttack))]
    public class Character : MonoBehaviour
    {

        public CharacterStats Stats { get; private set; }
        public CharacterEffects Effects { get; private set; }
        public CharacterAnimator CharAnimator { get; private set; }
        public CharacterAttack Attack { get; private set; }
        public CharacterFacing Facing { get; private set; }
        public CharacterDeath Death { get; private set; }

        public bool BattleStarted { get; private set; } = false;
        public bool IsDead => Death.IsDead;
        private CharacterUI _characterUI;
        private const float ALIVE_HEALTH_THRESHOLD = 0f;
        protected void Awake()
        {
            Stats = GetComponent<CharacterStats>();
            Effects = GetComponent<CharacterEffects>();
            CharAnimator = GetComponent<CharacterAnimator>();
            Attack = GetComponent<CharacterAttack>();
            Facing = GetComponent<CharacterFacing>();
            Death = GetComponent<CharacterDeath>();
            _characterUI = GetComponentInChildren<CharacterUI>();
            Effects.Initialize(this);
        }
        public void Initialize(ProjectileRegistry projectileRegistry, bool facingRight)
        {
            Attack.InitializeProjectileRegistry(projectileRegistry);
            Facing.SetFacingRight(facingRight);
            _characterUI.Initialize(this);
        }
        public void TakeDamage(float damage)
        {
            Stats.TakeDamage(damage);

            if (Stats.CurrentHealth <= ALIVE_HEALTH_THRESHOLD || IsDead)
            {
                Die();
            }
        }

        public void ApplyEffect(CharacterApplicableEffect effect)
        {
            if (IsDead) return;

            Effects.Apply(effect);

        }

        public bool CanAttack()
        {
            return !IsDead && BattleStarted && !Effects.HasEffect(EffectType.Stun);
        }

        public void StartBattle()
        {
            BattleStarted = true;

        }

        private void Die()
        {
            Death.Die();
        }

        public int FacingDirection()
        {
            return Facing.FacingDirection;
        }
    }
}