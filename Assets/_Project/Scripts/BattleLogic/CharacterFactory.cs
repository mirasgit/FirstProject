using System.Collections.Generic;
using UnityEngine;
using FirstProject.Characters;
using FirstProject.UI;
using Zenject;

namespace FirstProject.Battle
{
    public class CharacterFactory
    {
        private readonly List<Character> _characterPrefabs;
        private readonly IInstantiator _instantiator;

        public CharacterFactory(IInstantiator instantiator, List<Character> prefabs)
        {
            _instantiator = instantiator;
            _characterPrefabs = prefabs;
        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _characterPrefabs.Count);
            Character prefab = _characterPrefabs[randomIndex];
            Character model = _instantiator.InstantiatePrefabForComponent<Character>(prefab, spawnPoint.position, Quaternion.identity, null);
            model.SetFacingRight(facingRight);
            CharacterView view = model.GetComponentInChildren<CharacterView>(true);
            CharacterPresenter presenter = new(view, model);
            presenter.Subscribe();
            return model;
        }
    }
}