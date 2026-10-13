using Photon.Pun;
using System;
using UnityEngine;
using Zenject;

namespace FirstProject.Meta.Multiplayer
{
    public class NetworkCharacterSpawner : IPunPrefabPool, IInitializable, IDisposable
    {
        private readonly IInstantiator _instantiator;

        public NetworkCharacterSpawner(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public void Initialize()
        {
            PhotonNetwork.PrefabPool = this;
        }

        public void Dispose()
        {
            PhotonNetwork.PrefabPool = new DefaultPool();
        }

        public GameObject Instantiate(string prefabID, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabID);

            GameObject instance = _instantiator.InstantiatePrefab(prefab, position, rotation, null);

            instance.SetActive(false);

            return instance;
        }

        public void Destroy(GameObject gameObject)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
