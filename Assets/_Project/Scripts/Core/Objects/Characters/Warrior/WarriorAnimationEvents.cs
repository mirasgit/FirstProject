using UnityEngine;

namespace FirstProject.Core.Characters
{
    public class WarriorAnimationEvents : MonoBehaviour
    {
        private WarriorAttack _entity;

        private void Awake()
        {
            _entity = GetComponentInParent<WarriorAttack>();
        }

        public void DamageTargets()
        {
            _entity.DamageTargets();
        } 
    }
}