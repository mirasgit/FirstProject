using UnityEngine;

public class WizardEvents : MonoBehaviour
{
    private Wizard entity;
    private void Awake()
    {
        entity = GetComponentInParent<Wizard>();
    }
    public void SpawnProjectile() => entity.SpawnProjectile();

}
