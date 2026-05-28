using System.Collections.Generic;
using UnityEngine;
using FirstProject.Projectiles;
using FirstProject.Characters;
namespace FirstProject.Battle
{
    public class CharacterFactory : MonoBehaviour
    {
        [SerializeField] private List<Character> _characterPrefabs;
        private ProjectileRegistry _projectileRegistry;

        public void Initialize(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
        }
        public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
        {
            int randomIndex = Random.Range(0, _characterPrefabs.Count);
            Character prefab = _characterPrefabs[randomIndex];
            Character character = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            character.Initialize(_projectileRegistry, facingRight);
            

            return character;
        }
    }
}