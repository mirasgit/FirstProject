using UnityEngine;

public class BattleFlow : MonoBehaviour
{
    private CharacterFactory _characterFactory;
    private BattleView _battleView;
    private BattleCleanupService _battleCleanupService;
    private Transform _leftSpawnPoint;
    private Transform _rightSpawnPoint;
    private Character _leftCharacter;
    private Character _rightCharacter;
    private bool _battleStarted;
    private bool _battleFinished;
    public void Initialize(
        CharacterFactory characterFactory,
        BattleView battleView,
        BattleCleanupService battleCleanupService,
        Transform leftSpawnPoint,
        Transform rightSpawnPoint)
    {
        _characterFactory = characterFactory;
        _battleView = battleView;
        _battleCleanupService = battleCleanupService;
        _leftSpawnPoint = leftSpawnPoint;
        _rightSpawnPoint = rightSpawnPoint;
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
        if (_battleStarted == false) return;

        if (_battleFinished) return;

        if (_leftCharacter == null || _rightCharacter == null) return;

        if (_leftCharacter._isDead)
        {
            _battleFinished = true;
            _battleView.ShowWinner("Right won");
            
        }
        else if (_rightCharacter._isDead)
        {
            _battleFinished = true;
            _battleView.ShowWinner("Left won");
        }
    }
    public void StartBattle()
    {
        ClearBattle();
        SpawnCharacters();
        _battleView.ShowBattleScreen();
        _battleStarted = true;
    }

    public void RestartBattle()
    {
        StartBattle();
    }

    private void SpawnCharacters()
    {
        _leftCharacter = _characterFactory.SpawnRandomCharacter(_leftSpawnPoint, true);
        _rightCharacter = _characterFactory.SpawnRandomCharacter(_rightSpawnPoint, false);
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
        _battleStarted = false;
        _battleFinished = false;
    }
}
