using FirstProject.Core.Battle;
using Zenject;

namespace FirstProject.Core.Projectiles
{
    public class ProjectileFactory : BaseFactory<Projectile>
    {

        public ProjectileFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }

    }
}
