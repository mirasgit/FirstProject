using FirstProject.Projectiles;
using FirstProject.Characters;
using FirstProject.UI;
using System.Collections.Generic;

namespace FirstProject.Battle
{
    public class BattleCleanupService
    {
        private IEnumerable<IClearableRegistry> _registries;

        public BattleCleanupService(IEnumerable<IClearableRegistry> registries)
        {
            _registries = registries; 
        }

        public void ClearAllTemporaryObjects()
        {
            foreach (var registry in _registries)
            {
                registry.ClearAll();
            }
        }

        public void DestroyCharacter(Character character)
        {
            if (character == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(character.gameObject);
        }
    }
}