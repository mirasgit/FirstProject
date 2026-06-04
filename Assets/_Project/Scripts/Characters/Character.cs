using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters.Attack;   
using FirstProject.Projectiles;
using FirstProject.UI;
using System;

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

        private CharacterStats _stats;
        private CharacterEffects _effects;
        private CharacterAnimator _charAnimator;
        private CharacterAttack _attack;
        private CharacterFacing _facing;
        private CharacterDeath _death;

        public bool BattleStarted { get; private set; } = false;
        public bool IsDead => _death.IsDead;
        private CharacterUI _characterUI;
        private const float ALIVE_HEALTH_THRESHOLD = 0f;
        public event Action Died;

        #region Dependants Events 
        public event Action<float, float> HealthChanged 
        {
            add => _stats.HealthChanged += value;
            remove => _stats.HealthChanged -= value;
        }
        public event Action<float> DamageTaken
        {
            add => _stats.DamageTaken += value;
            remove => _stats.DamageTaken -= value;
        }
        public event Action<CharacterApplicableEffect> EffectApplied
        {
            add => _effects.EffectApplied += value;
            remove => _effects.EffectApplied -= value;
        }
        public event Action EffectEnded
        {
            add => _effects.EffectEnded += value; 
            remove => _effects.EffectEnded -= value;
        }
        #endregion

        protected void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            _effects = GetComponent<CharacterEffects>();
            _charAnimator = GetComponent<CharacterAnimator>();
            _attack = GetComponent<CharacterAttack>();
            _facing = GetComponent<CharacterFacing>();
            _death = GetComponent<CharacterDeath>();
            _characterUI = GetComponentInChildren<CharacterUI>();
            _effects.Initialize(this);
        }
        public void Initialize(ProjectileRegistry projectileRegistry, bool facingRight)
        {
            _attack.InitializeProjectileRegistry(projectileRegistry);
            _facing.SetFacingRight(facingRight);
            _characterUI.Initialize(this);
        }
        #region Character Dependants
        public void TakeDamage(float damage)
        {
            _stats.TakeDamage(damage);

            if (_stats.CurrentHealth <= ALIVE_HEALTH_THRESHOLD || IsDead)
            {
                Die();
            }

        }

        public float GetCurrentDamage()
        {
            return _stats.CurrentDamage;
        }
        public void ChangeDamageTo(float damage)
        {
            _stats.ChangeDamage(damage);
        }
        public float GetCurrentHealth()
        {
            return _stats.CurrentHealth;
        }
        public float GetMaxHealth()
        {
            return _stats.MaxHealth;
        }

        public void ApplyEffect(CharacterApplicableEffect effect)
        {
            if (IsDead) return;

            _effects.Apply(effect);
            
        }

        public void PlayAttack()
        {
            _charAnimator.PlayAttack();
        }

        public void EnablePlayStun(bool enable)
        {
            if (enable)
            {
                _charAnimator.PlayStun();
            }
            else
            {
                _charAnimator.StopPlayStun();
            }
        }
        public void ToggleAttack(bool enable)
        {
            _charAnimator.ToggleAttack(enable);
        }
        public void SetVelocity(float parameter)
        {
            _charAnimator.SetVelocity(parameter);
        }

        #endregion
        #region Character Own Methods
        public bool CanAttack()
        {
            return !IsDead && BattleStarted && !_effects.HasEffect(EffectType.Stun);
        }

        public void StartBattle()
        {
            BattleStarted = true;

        }

        private void Die()
        {
            _death.Die();
            Died?.Invoke();
        }
        
        public int FacingDirection()
        {
            return _facing.FacingDirection;
        }
#endregion
    }
}