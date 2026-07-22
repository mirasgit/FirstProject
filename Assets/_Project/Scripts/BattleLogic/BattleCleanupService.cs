using FirstProject.Projectiles;
using FirstProject.Characters;
using FirstProject.UI;

namespace FirstProject.Battle
{
    public class BattleCleanupService
    {
        private readonly ProjectileRegistry _projectileRegistry;
        private readonly FloatingTextRegistry _floatingTextRegistry;
        public BattleCleanupService(ProjectileRegistry projectileRegistry, FloatingTextRegistry floatingTextRegistry)
        {
            _projectileRegistry = projectileRegistry;
            _floatingTextRegistry = floatingTextRegistry;
        }

        public void ClearAllProjectiles()
        {
            _projectileRegistry.DestroyAll();
            _floatingTextRegistry.DestroyAll();
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