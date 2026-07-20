using System;
using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [field: SerializeField] public float BaseDamage { get; private set; } = 10;
        [field: SerializeField] public float BaseHealth { get; private set; } = 100;
        public float CurrentHealth { get; private set; }
        public float CurrentDamage => BaseDamage + (BaseDamage*_damageModifiersSum);

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;
        private float _damageModifiersSum;

        private void Awake()
        {
            CurrentHealth = BaseHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
            HealthChanged?.Invoke(CurrentHealth, BaseHealth);
            DamageTaken?.Invoke(damage);
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