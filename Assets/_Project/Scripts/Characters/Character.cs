using UnityEngine;
using FirstProject.CharacterEffect;
using System;
using Zenject;
using FirstProject.MatchupConfigs;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(CharacterEffects))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(CharacterDeath))]
    [RequireComponent(typeof(CharacterFacing))]
    [RequireComponent(typeof(CharacterAttack))]
    public class Character : MonoBehaviour, IDamageable, IKillable
    {
        private CharacterStats _stats;
        private CharacterEffects _effects;
        private CharacterAttack _attack;
        private CharacterFacing _facing;
        private CharacterDeath _death;

        private const float ALIVE_HEALTH_THRESHOLD = 0f;
        public bool IsDead => _death.IsDead;
        public float CurrentHealth => _stats.CurrentHealth;
        public float MaxHealth => _stats.MaxHealth;

        public event Action Died;
        public event Action Destroyed;
        
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

        public event Action<CharacterApplicableEffect> EffectEnded
        {
            add => _effects.EffectEnded += value; 
            remove => _effects.EffectEnded -= value;
        }

        [Inject]
        private void Construct(CharacterStats stats, CharacterEffects effects, CharacterAttack attack, CharacterFacing facing, CharacterDeath death)
        {
            _stats = stats;
            _effects = effects;
            _attack = attack;
            _facing = facing;
            _death = death;
        }

        public void ApplyUpgrades(float healthMultiplier, float damageMultiplier, float attackSpeedMultiplier)
        {
            _stats.AddHealthModifier(healthMultiplier);
            _stats.AddDamageModifier(damageMultiplier);
            _attack.ApplyMultiplier(attackSpeedMultiplier);
        }

        public void SetFacingRight(bool facingRight)
        {
            _facing.SetFacingRight(facingRight);
        }
        
        public void TakeDamage(float damage, CharacterClass attackerClass)
        {
            _stats.TakeDamage(damage, attackerClass);

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

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}