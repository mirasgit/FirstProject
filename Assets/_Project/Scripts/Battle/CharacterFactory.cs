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
        private readonly ProgressModel _model;

        public CharacterFactory(IInstantiator instantiator, List<Character> prefabs, ProgressModel model)
        {
            _instantiator = instantiator;
            _characterPrefabs = prefabs;
            _model = model;
        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _characterPrefabs.Count);
            Character prefab = _characterPrefabs[randomIndex];
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