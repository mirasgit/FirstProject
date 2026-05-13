using UnityEngine;


public class RangerEvents : MonoBehaviour
{
    private ArcherAttack _entity;
    private void Awake()
    {
        _entity = GetComponentInParent<ArcherAttack>();
    }
    public void SpawnProjectile() => _entity.SpawnProjectile();

}