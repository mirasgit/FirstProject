using System;
using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [field: SerializeField] private float _maxHealth = 100;
        [field: SerializeField] private float _maxDamage = 10;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth { get; private set; }
        public float CurrentDamage { get; private set; }

        public event Action<float, float> HealthChanged;
        public event Action<float> DamageTaken;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
            CurrentDamage = _maxDamage;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
            HealthChanged?.Invoke(CurrentHealth, _maxHealth);
            DamageTaken?.Invoke(damage);
        }

        public void ChangeDamage(float damage)
        {
            CurrentDamage = damage;
        }
    }
}