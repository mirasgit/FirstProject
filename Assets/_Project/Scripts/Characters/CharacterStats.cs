using FirstProject.Configs;
using FirstProject.MatchupConfigs;
using System;
using UnityEngine;
using Zenject;

namespace FirstProject.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        public float BaseDamage { get; private set; }
        public float BaseHealth { get; private set; }

        public float MaxHealth => BaseHealth + (BaseHealth * _healthModifiersSum);
        public float CurrentHealth { get; private set; }
        public float CurrentDamage => BaseDamage + (BaseDamage * _damageModifiersSum);

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;
        private RemoteConfigService _configService;
        private CharacterIdentity _identity;
        private float _damageModifiersSum;
        private float _healthModifiersSum;

        [Inject]
        public void Construct(RemoteConfigService configService, CharacterIdentity identity)
        {
            _configService = configService;
            _identity = identity;

            var config = _configService.GetCharacterConfig(_identity.MyClass);

            BaseHealth = config.BaseHealth;
            BaseDamage = config.BaseDamage;
            CurrentHealth = MaxHealth;
        }

        private float GetMultiplier(CharacterClass attacker, CharacterClass defender)
        {
            foreach (var matchup in _configService.Data.Matchups)
            {
                if (matchup.Attacker == attacker && matchup.Defender == defender)
                {
                    return matchup.DamageMultiplier;
                }
            }
            return _configService.Data.Upgrades.DefaultMultiplier;
        }

        public void AddHealthModifier(float modifier)
        {
            _healthModifiersSum += modifier;

            CurrentHealth = MaxHealth;

            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float damage, CharacterClass attackerClass)
        {
            float multiplier = GetMultiplier(attackerClass, _identity.MyClass);
            float finalDamage = multiplier * damage;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - finalDamage);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
            DamageTaken?.Invoke(finalDamage);
        }

        public void AddDamageModifier(float modifier)
        {
            _damageModifiersSum += modifier;
        }

        public void RemoveDamageModifier(float modifier)
        {
            _damageModifiersSum -= modifier;
        }
    }
}