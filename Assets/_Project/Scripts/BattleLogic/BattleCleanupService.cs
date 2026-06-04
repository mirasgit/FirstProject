using FirstProject.Projectiles;
using FirstProject.Characters;

namespace FirstProject.Battle
{
    public class BattleCleanupService
    {
        private readonly ProjectileRegistry _projectileRegistry;

        public BattleCleanupService(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
        }
        public void ClearAllProjectiles()
        {
            _projectileRegistry.DestroyAll();
        }
        public void DestroyCharacter(Character character)
        {
            if (character == null)
                return;


            UnityEngine.Object.Destroy(character.gameObject);
        }
    }
}