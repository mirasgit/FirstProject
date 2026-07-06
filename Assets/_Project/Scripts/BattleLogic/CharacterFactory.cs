using System.Collections.Generic;
using UnityEngine;
using FirstProject.Projectiles;
using FirstProject.Characters;
using FirstProject.UI;

namespace FirstProject.Battle
{
    public class CharacterFactory
    {
        private readonly List<Character> _characterPrefabs;
        private readonly ProjectileRegistry _projectileRegistry;

        public CharacterFactory(ProjectileRegistry projectileRegistry, List<Character> prefabs)
        {
            _projectileRegistry = projectileRegistry;
            _characterPrefabs = prefabs;
        }

        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _characterPrefabs.Count);
            Character prefab = _characterPrefabs[randomIndex];
            Character model = Object.Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            model.Initialize(_projectileRegistry, facingRight);
            CharacterView view = model.GetComponentInChildren<CharacterView>(true);
            new CharacterPresenter(view, model);

            return model;
        }
    }
}