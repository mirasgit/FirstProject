using Unity.VisualScripting;
using UnityEngine;

public class BattleFlow : MonoBehaviour
{
    private CharacterFactory _characterFactory;
    private BattleView _battleView;
    private BattleCleanupService _battleCleanupService;
    private Transform _leftSpawnPoint;
    private Transform _rightSpawnPoint;
    private ProjectileRegistry _projectileRegistry;

    private Character _leftCharacter;
    private Character _rightCharacter;

    public void Initialize(
        CharacterFactory characterFactory,
        BattleView battleView,
        BattleCleanupService battleCleanupService,
        Transform leftSpawnPoint,
        Transform rightSpawnPoint, 
        ProjectileRegistry projectileRegistry)
    {
        _characterFactory = characterFactory;
        _battleView = battleView;
        _battleCleanupService = battleCleanupService;
        _leftSpawnPoint = leftSpawnPoint;
        _rightSpawnPoint = rightSpawnPoint;
        _projectileRegistry = projectileRegistry;
    }
    private void Start()
    {
        _battleView.ShowStartScreen();
    }
    private void Update()
    {
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (_leftCharacter == null || _rightCharacter == null) return;
        if (_leftCharacter._isDead)
        {
            _battleView.ShowWinner("Right won");
            
        }
        else if (_rightCharacter._isDead)
        {
            _battleView.ShowWinner("Left won");
        }
    }
    public void StartBattle()
    {
        ClearBattle();
        SpawnCharacters();
        _battleView.ShowBattleScreen();
    }

    public void RestartBattle()
    {
        StartBattle();
    }

    private void SpawnCharacters()
    {
        _leftCharacter = _characterFactory.SpawnRandomCharacter(_leftSpawnPoint);
        _rightCharacter = _characterFactory.SpawnRandomCharacter(_rightSpawnPoint);
        _rightCharacter.FacingRight = false;
        _leftCharacter.InitializeProjectileRegistry(_projectileRegistry);
        _rightCharacter.InitializeProjectileRegistry(_projectileRegistry);
        _rightCharacter.StartBattle();
        _leftCharacter.StartBattle();
    }
    private void ClearBattle()
    {
        _battleCleanupService.DestroyCharacter(_leftCharacter);
        _battleCleanupService.DestroyCharacter(_rightCharacter);
        _battleCleanupService.ClearAllProjectiles();

        _leftCharacter = null;
        _rightCharacter = null;
    }
}
