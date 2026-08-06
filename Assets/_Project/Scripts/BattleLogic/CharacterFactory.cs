using System.Collections.Generic;
using UnityEngine;
using FirstProject.Characters;
using FirstProject.UI;
using Zenject;
using FirstProject.Shop;

namespace FirstProject.Battle
{
    public class CharacterFactory
    {
        private readonly List<Character> _characterPrefabs;
        private readonly IInstantiator _instantiator;
        private readonly SaveService _saveService;

        public CharacterFactory(IInstantiator instantiator, List<Character> prefabs, SaveService saveService)
        {
            _instantiator = instantiator;
            _characterPrefabs = prefabs;
            _saveService = saveService;
        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _characterPrefabs.Count);
            Character prefab = _characterPrefabs[randomIndex];
            Character model = _instantiator.InstantiatePrefabForComponent<Character>(prefab, spawnPoint.position, Quaternion.identity, null);
            model.SetFacingRight(facingRight);
            if (facingRight)
            {
                model.ApplyUpgrades(_saveService.GetHealthMP(), _saveService.GetDamageMP(), _saveService.GetAttackSpeedMP());
            }
            CharacterView view = model.GetComponentInChildren<CharacterView>(true);
            CharacterPresenter presenter = new(view, model);
            presenter.Subscribe();
            return model;
        }
    }
}