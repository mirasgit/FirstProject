using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 100;
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [field: SerializeField] public float MaxDamage { get; private set; } = 10;
    [field: SerializeField] public float CurrentDamage {  get; private set; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentDamage = MaxDamage;
    }

    public void ResetCharacterStats()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        
    }
    public void ChangeDamage(float damage)
    {
        CurrentDamage = damage;
    }
}
