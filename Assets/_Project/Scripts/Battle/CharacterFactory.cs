using System.Collections.Generic;
using UnityEngine;
using FirstProject.Characters;
using Zenject;
using FirstProject.Shop;
using FirstProject.Characters.UI;
using FirstProject.Core;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace FirstProject.Battle
{
    public class CharacterFactory : IDisposable
    {
        private readonly IResourceProvider _resourceProvider;   
        private readonly IInstantiator _instantiator;
        private readonly ProgressModel _model;

        private readonly List<string> _characterAddressKeys = new() { "Warrior", "Archer", "Wizard" };

        private readonly List<Character> _loadedPrefabs = new();


        public CharacterFactory(IInstantiator instantiator, IResourceProvider resourceProvider, ProgressModel model)
        {
            _instantiator = instantiator;
            _resourceProvider = resourceProvider;
            _model = model;
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
            Character characterInstance = _instantiator.InstantiatePrefabForComponent<Character>(prefab, spawnPoint.position, Quaternion.identity, null);
            characterInstance.SetFacingRight(facingRight);
            if (facingRight)
            {
                characterInstance.ApplyUpgrades(_model.HealthMultiplier, _model.DamageMultiplier, _model.AttackSpeedMultiplier);
            }
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