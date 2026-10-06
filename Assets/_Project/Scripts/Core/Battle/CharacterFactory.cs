using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using FirstProject.Meta.Shop;
using FirstProject.Core.Characters;
using FirstProject.UI.Characters;
using Photon.Pun;

namespace FirstProject.Core.Battle
{
    public class CharacterFactory : IDisposable
    {
        private readonly IResourceProvider _resourceProvider;   
        private readonly DiContainer _container;
        private readonly ProgressModel _progressModel;

        private readonly List<string> _characterAddressKeys = new() { "Warrior", "Archer", "Wizard" };

        private readonly List<Character> _loadedPrefabs = new();

        public CharacterFactory(DiContainer container, IResourceProvider resourceProvider, ProgressModel model)
        {
            _container = container;
            _resourceProvider = resourceProvider;
            _progressModel = model;
        }

        public async UniTask LoadCharactersAsync(CancellationToken token = default)
        {
            if (_loadedPrefabs.Count > 0)
            {
                return;
            }

            var loadTasks = new List<UniTask<GameObject>>();

            foreach (var key in _characterAddressKeys)
            {
                loadTasks.Add(_resourceProvider.LoadAssetAsync<GameObject>(key, token));
            }

            GameObject[] loadedObjects = await UniTask.WhenAll(loadTasks);

            foreach (var obj in loadedObjects)
            {
                _loadedPrefabs.Add(obj.GetComponent<Character>());
            }
        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = UnityEngine.Random.Range(0, _loadedPrefabs.Count);
            Character prefab = _loadedPrefabs[randomIndex];
            Character characterInstance = _container.InstantiatePrefabForComponent<Character>(prefab, spawnPoint.position, Quaternion.identity, null);
            if (facingRight)
            {
                characterInstance.ApplyUpgrades(_progressModel.HealthMultiplier, _progressModel.DamageMultiplier, _progressModel.AttackSpeedMultiplier);
            }
            characterInstance.SetFacingRight(facingRight);
            CharacterView view = characterInstance.GetComponentInChildren<CharacterView>(true);
            CharacterPresenter presenter = new(view, characterInstance);
            presenter.Subscribe();
            return characterInstance;
        }

        public Character SpawnRandomNetworkCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = UnityEngine.Random.Range(0, _characterAddressKeys.Count);
            object[] spawnData = new object[] { 
                facingRight,
                _progressModel.HealthMultiplier,
                _progressModel.DamageMultiplier,
                _progressModel.AttackSpeedMultiplier,            
            };

            GameObject networkObject = PhotonNetwork.Instantiate(_characterAddressKeys[randomIndex], spawnPoint.position, Quaternion.identity, 0, spawnData);
            Character characterInstance = networkObject.GetComponent<Character>();

            CharacterView view = characterInstance.GetComponentInChildren<CharacterView>(true);
            CharacterPresenter presenter = new(view, characterInstance);
            presenter.Subscribe();
            return characterInstance;
        }

        public void Dispose()
        {
            foreach (var prefab in _loadedPrefabs)
            {
                if (prefab != null)
                {
                    _resourceProvider.ReleaseAsset(prefab.gameObject);
                }
            }
            _loadedPrefabs.Clear();
        }
    }
}