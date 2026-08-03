using FirstProject.Battle;
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
        public float CurrentHealth { get; private set; }
        public float CurrentDamage => BaseDamage + (BaseDamage*_damageModifiersSum);

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;
        private MatchupMatrixConfig _matchupMatrix;
        private float _damageModifiersSum;

        private void Awake()
        {
            CurrentHealth = BaseHealth;
        }

        [Inject]
        public void Construct(MatchupMatrixConfig matrixConfig)
        {
            _matchupMatrix = matrixConfig;
        }

        public void TakeDamage(float damage, CharacterClass attackerClass)
        {
            float multiplier = _matchupMatrix.GetMultiplier(attackerClass, MyClass);
            float finalDamage = multiplier * damage;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - finalDamage);
            HealthChanged?.Invoke(CurrentHealth, BaseHealth);
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