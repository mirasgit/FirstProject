using UnityEngine;
using FirstProject.Characters.Attack;

namespace FirstProject.Characters
{
    public class ArcherAnimationEvents : MonoBehaviour
    {
        private ArcherAttack _entity;

        private void Awake()
        {
            _entity = GetComponentInParent<ArcherAttack>();
        }

        public void SpawnProjectile()
        {
            _entity.SpawnProjectile();
        }
    }
}