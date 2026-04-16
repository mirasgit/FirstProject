using Mono.Cecil.Cil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    [SerializeField] private List<Character> _characterPrefabs;

    [Header("Spawnpoints")]
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;

    [Header("UI")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private Button _restartButton;
    private Character _leftCharacter;
    private Character _rightCharacter;

    private void Awake()
    {
        _startButton.onClick.AddListener(StartBattle);
        _restartButton.onClick.AddListener(RestartBattle);

        _startPanel.SetActive(true);
        _winPanel.SetActive(false);
    }

    public void RestartBattle()
    {
        StartBattle();
    }

    private void Update()
    {
        if (_leftCharacter == null || _rightCharacter == null) return;
        if (_leftCharacter._isDead)
        {
            ShowWinner("Right won");
        }
        else if (_rightCharacter._isDead)
        {
            ShowWinner("Left won");
        }
    }
    private void ShowWinner(string winnerName)
    {
        if (_winPanel.activeSelf) return;

        _winnerText.text = $"Победил: {winnerName}";
        _winPanel.SetActive(true);
    }
    private Character SpawnRandomCharacter(Transform spawnpoint)
    {
        int randomIndex = Random.Range(0, _characterPrefabs.Count);
        Character prefab = _characterPrefabs[randomIndex];

        Character spawnedCharacter = Instantiate(prefab, spawnpoint.position, spawnpoint.rotation);
        return spawnedCharacter;
    }
    public void StartBattle()
    {
        ClearOldCharacters();
        ClearAllProjectiles();

        _startPanel.SetActive(false);
        _winPanel.SetActive(false);

        _leftCharacter = SpawnRandomCharacter(_leftSpawnPoint);
        _rightCharacter = SpawnRandomCharacter(_rightSpawnPoint);
        _rightCharacter._facingRight = false;
        _leftCharacter.StartBattle();
        _rightCharacter.StartBattle();
    }
    private void ClearOldCharacters()
    {
        if (_leftCharacter != null)
            Destroy(_leftCharacter.gameObject);
        if (_rightCharacter != null)
            Destroy(_rightCharacter.gameObject);
    }
    private void ClearAllProjectiles()
    {
        Projectile[] projectiles = FindObjectsByType<Projectile>(FindObjectsSortMode.None);

        foreach (Projectile projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }
    }
}
