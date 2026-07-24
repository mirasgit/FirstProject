using UnityEngine;

namespace FirstProject.Characters
{
    public class RangerAnimationEvents : MonoBehaviour
    {
        private RangedAttack _entity;

        private void Awake()
        {
            _entity = GetComponentInParent<RangedAttack>();
        }

        public void SpawnProjectile()
        {
            _entity.SpawnProjectile();
        }
    }
}