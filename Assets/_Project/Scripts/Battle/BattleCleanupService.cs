using FirstProject.Characters;
using System.Collections.Generic;

namespace FirstProject.Battle
{
    public class BattleCleanupService
    {
        private readonly IEnumerable<IClearableRegistry> _registries;

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