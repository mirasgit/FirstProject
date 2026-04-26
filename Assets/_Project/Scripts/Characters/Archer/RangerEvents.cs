using UnityEngine;


public class RangerEvents : MonoBehaviour
{
    private ArcherAttack entity;
    private void Awake()
    {
        entity = GetComponentInParent<ArcherAttack>();
    }
    public void SpawnProjectile() => entity.SpawnProjectile();

}