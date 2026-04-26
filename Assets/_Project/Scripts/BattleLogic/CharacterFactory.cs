using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private List<Character> _characterPrefabs;
    [SerializeField] private ProjectileRegistry _projectileRegistry;

    public Character SpawnRandomCharacter(Transform spawnPoint, bool facingRight)
    {
        int randomIndex = Random.Range(0, _characterPrefabs.Count);
        Character prefab = _characterPrefabs[randomIndex];

        Character character = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        character.SetFacingRight(facingRight);
        character.InitializeProjectileRegistry(_projectileRegistry);

        return character;
    }
}
