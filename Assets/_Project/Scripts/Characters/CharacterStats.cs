using System;
using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [field: SerializeField] public float MaxDamage { get; private set; } = 10;
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
        public float CurrentHealth { get; private set; }
        public float CurrentDamage { get; private set; }

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            CurrentDamage = MaxDamage;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
            DamageTaken?.Invoke(damage);
        }

        public void ChangeDamage(float damage)
        {
            CurrentDamage = damage;
        }
    }
}