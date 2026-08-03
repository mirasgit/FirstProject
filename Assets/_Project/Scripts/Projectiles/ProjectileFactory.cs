using Zenject;
using FirstProject.Battle;

namespace FirstProject.Projectiles
{
    public class ProjectileFactory : BaseFactory<Projectile>
    {

        public ProjectileFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }

    }
}
