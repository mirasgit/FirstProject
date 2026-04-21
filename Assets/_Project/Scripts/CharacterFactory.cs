using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private List<Character> _characterPrefabs;

    public Character SpawnRandomCharacter(Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, _characterPrefabs.Count);
        Character prefab = _characterPrefabs[randomIndex];

        Character character = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        return character;
    }
}