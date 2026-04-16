using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RangerEvents : MonoBehaviour
{
    private Ranger entity;
    private void Awake()
    {
        entity = GetComponentInParent<Ranger>();
    }
    public void SpawnProjectile() => entity.SpawnProjectile();

}