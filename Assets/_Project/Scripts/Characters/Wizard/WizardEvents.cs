using UnityEngine;
using FirstProject.Characters.Attack;

namespace FirstProject.Characters
{
    public class WizardEvents : MonoBehaviour
    {
        private WizardAttack _entity;
        private void Awake()
        {
            _entity = GetComponentInParent<WizardAttack>();
        }
        public void SpawnProjectile() => _entity.SpawnProjectile();

    }
}