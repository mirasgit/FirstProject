using System;

namespace FirstProject.Characters
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        event Action Died;
    }
}

