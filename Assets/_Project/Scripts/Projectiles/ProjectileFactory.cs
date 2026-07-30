using UnityEngine;
using Zenject;

namespace FirstProject.Projectiles
{
    public class ProjectileFactory
    {
        private IInstantiator _instantiator;

        public ProjectileFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public Projectile Create(Projectile prefab, Vector3 position, Quaternion rotation)
        {
            return _instantiator.InstantiatePrefabForComponent<Projectile>(prefab, position, rotation, null);
        }
    }
}
