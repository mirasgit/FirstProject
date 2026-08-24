using Zenject;
using FirstProject.Core;

namespace FirstProject.Projectiles
{
    public class ProjectileFactory : BaseFactory<Projectile>
    {

        public ProjectileFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }

    }
}
