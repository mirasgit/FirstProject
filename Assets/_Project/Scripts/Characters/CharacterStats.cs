using FirstProject.MatchupConfigs;
using System;
using UnityEngine;
using Zenject;

namespace FirstProject.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [field: SerializeField] public float BaseDamage { get; private set; } = 10;
        [field: SerializeField] public float BaseHealth { get; private set; } = 100;

        [field: SerializeField] public CharacterClass MyClass { get; private set; }

        public float MaxHealth => BaseHealth + (BaseHealth * _healthModifiersSum);
        public float CurrentHealth { get; private set; }
        public float CurrentDamage => BaseDamage + (BaseDamage*_damageModifiersSum);

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;
        private MatchupMatrixConfig _matchupMatrix;
        private float _damageModifiersSum;
        private float _healthModifiersSum;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        [Inject]
        public void Construct(MatchupMatrixConfig matrixConfig)
        {
            _matchupMatrix = matrixConfig;
        }

        public void AddHealthModifier(float modifier)
        {
            _healthModifiersSum += modifier;

            CurrentHealth = MaxHealth;

            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float damage, CharacterClass attackerClass)
        {
            float multiplier = _matchupMatrix.GetMultiplier(attackerClass, MyClass);
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