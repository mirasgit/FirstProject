using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters.Attack;   
using FirstProject.Projectiles;
using System;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
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
        private CharacterEffectContext _context;

        private const float ALIVE_HEALTH_THRESHOLD = 0f;
        public bool IsDead => _death.IsDead;
        public float CurrentHealth => _stats.CurrentHealth;
        public float MaxHealth => _stats.MaxHealth;

        public event Action Died;
        
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

        private void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            _effects = GetComponent<CharacterEffects>();
            _charAnimator = GetComponent<CharacterAnimator>();
            _attack = GetComponent<CharacterAttack>();
            _facing = GetComponent<CharacterFacing>();
            _death = GetComponent<CharacterDeath>();
            _context = new CharacterEffectContext(_stats, _charAnimator, TakeDamage);
            _effects.SetContext(_context);
        }

        public void Initialize(ProjectileRegistry projectileRegistry, bool facingRight)
        {
            _attack.Initialize(_stats, _facing, _charAnimator, _death, _effects, projectileRegistry);
            _facing.SetFacingRight(facingRight);
        }
        
        public void TakeDamage(float damage)
        {
            _stats.TakeDamage(damage);

            if (_stats.CurrentHealth <= ALIVE_HEALTH_THRESHOLD)
            {
                Die();
            }
        }

        public void ApplyEffect(CharacterApplicableEffect effect)
        {
            if (IsDead)
            {
                return;
            }

            _effects.Apply(effect);
        }

        public void AllowToFight()
        {
            _attack.AllowToFight();
        }

        public void DisallowToFight()
        {
            _attack.DisallowToFight();
        }

        private void Die()
        {
            if (_death.Die())
            {
                Died?.Invoke();
            }
        }
    }
}