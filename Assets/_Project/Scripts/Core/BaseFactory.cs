using Zenject;
using UnityEngine;

namespace FirstProject.Battle
{
    public class BaseFactory<T> where T : MonoBehaviour
    {
        private readonly IInstantiator _instantiator;

        public BaseFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public T Create(T prefab, Vector3 position, Quaternion rotation)
        {
            return _instantiator.InstantiatePrefabForComponent<T>(prefab, position, rotation, null);
        }
    }
}