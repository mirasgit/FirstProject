using System.Collections.Generic;
using UnityEngine;
using FirstProject.Characters;
using Zenject;
using FirstProject.Shop;
using FirstProject.Characters.UI;
using FirstProject.Core;
using Cysharp.Threading.Tasks;

namespace FirstProject.Battle
{
    public class CharacterFactory
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

        public async UniTask LoadCharactersAsync()
        {
            if (_loadedPrefabs.Count > 0)
            {
                return;
            }

            var loadTasks = new List<UniTask<GameObject>>();
            foreach (var key in _characterAddressKeys)
            {
                loadTasks.Add(_resourceProvider.LoadAssetAsync<GameObject>(key));
            }

            GameObject[] loadedObjects = await UniTask.WhenAll(loadTasks);

            foreach (var obj in loadedObjects)
            {
                _loadedPrefabs.Add(obj.GetComponent<Character>());
            }

        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _loadedPrefabs.Count);
            Character prefab = _loadedPrefabs[randomIndex];
            Character model = _instantiator.InstantiatePrefabForComponent<Character>(prefab, spawnPoint.position, Quaternion.identity, null);
            model.SetFacingRight(facingRight);
            if (facingRight)
            {
                model.ApplyUpgrades(_model.HealthMultiplier, _model.DamageMultiplier, _model.AttackSpeedMultiplier);
            }
            CharacterView view = model.GetComponentInChildren<CharacterView>(true);
            CharacterPresenter presenter = new(view, model);
            presenter.Subscribe();
            return model;
        }
    }
}