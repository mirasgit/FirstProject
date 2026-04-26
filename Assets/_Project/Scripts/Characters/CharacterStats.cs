using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: SerializeField] public float _maxHealth { get; private set; } = 100;
    [field: SerializeField] public float _currentHealth { get; private set; }
    [field: SerializeField] public float _maxDamage { get; private set; } = 10;
    [field: SerializeField] public float _currentDamage {  get; private set; }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _currentDamage = _maxDamage;
    }

    public void ResetCharacterStats()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("Stats take damage, HP:" + _currentHealth);
        _currentHealth -= damage;
        Debug.Log("Stats took damage, HP:" + _currentHealth);
        
    }
    public void ChangeDamage(float damage)
    {
        _currentDamage = damage;
    }
}
