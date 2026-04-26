using UnityEngine;

public class WarriorEvents : MonoBehaviour
{
    private WarriorAttack _entity;
    private void Awake()
    {
        _entity = GetComponentInParent<WarriorAttack>();
    }

    public void DamageTargets() => _entity.DamageTargets();
    private void DisableMovement() => _entity.EnableMovement(false);
    private void EnableMovement() => _entity.EnableMovement(true);
}