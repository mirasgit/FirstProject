using Zenject;
using UnityEngine;

namespace FirstProject.UI
{
    public class FloatingTextFactory
    {
        private IInstantiator _instantiator;

        public FloatingTextFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public FloatingText Create(FloatingText prefab, Vector3 position, Quaternion rotation)
        {
            return _instantiator.InstantiatePrefabForComponent<FloatingText>(prefab, position, rotation, null);
        }
    }

}