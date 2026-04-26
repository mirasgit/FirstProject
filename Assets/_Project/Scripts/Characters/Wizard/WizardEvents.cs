using UnityEngine;

public class WizardEvents : MonoBehaviour
{
    private WizardAttack entity;
    private void Awake()
    {
        entity = GetComponentInParent<WizardAttack>();
    }
    public void SpawnProjectile() => entity.SpawnProjectile();

}
