using FirstProject.MatchupConfigs;

namespace FirstProject.Core.Characters
{
    public interface IDamageable
    {
        void TakeDamage(float damage, CharacterClass attackerClass);

    }
}

