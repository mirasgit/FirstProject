using UnityEngine;
using FirstProject.UI;
using FirstProject.Characters;

namespace FirstProject.Battle
{
    public class BattleFlow
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleView _battleView;
        private readonly BattleCleanupService _battleCleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;
        private Character _leftCharacter;
        private Character _rightCharacter;
        private BattleState _state;

        public BattleFlow(
           CharacterFactory characterFactory,
           BattleView battleView,
           BattleCleanupService cleanupService,
           Transform leftSpawnPoint,
           Transform rightSpawnPoint)
        {
            _characterFactory = characterFactory;
            _battleView = battleView;
            _battleCleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
        }

        public void ShowStartScreen()
        {
            _state = BattleState.StartScreen;
            _battleView.ShowStartScreen();
        }

        public void StartBattle()
        {
            ClearBattle();
            _state = BattleState.Running;
            SpawnCharacters();
            _battleView.ShowBattleScreen();
        }

        public void RestartBattle()
        {
            StartBattle();
        }

        private void SpawnCharacters()
        {
            _leftCharacter = _characterFactory.SpawnRandomCharacter(_leftSpawnPoint, true);
            _rightCharacter = _characterFactory.SpawnRandomCharacter(_rightSpawnPoint, false);
            _rightCharacter.Died += OnCharacterDied;
            _leftCharacter.Died += OnCharacterDied;
            _rightCharacter.AllowToFight();
            _leftCharacter.AllowToFight();
        }

        private void OnCharacterDied()
        {
            if (_state != BattleState.Running)
            {
                return;
            }

            if (_leftCharacter == null || _rightCharacter == null)
            {
                return;
            }

            _leftCharacter.DisallowToFight();
            _rightCharacter.DisallowToFight();

            if (_leftCharacter.IsDead)
            {
                _state = BattleState.Finished;
                _battleView.ShowWinner(BattleResult.RightWon);
            }
            else if (_rightCharacter.IsDead)
            {
                _state = BattleState.Finished;
                _battleView.ShowWinner(BattleResult.LeftWon);
            }
        }

        private void ClearBattle()
        {
            if (_rightCharacter != null)
            {
                _rightCharacter.Died -= OnCharacterDied;
            }

            if (_leftCharacter != null)
            {
                _leftCharacter.Died -= OnCharacterDied;
            }

            _battleCleanupService.DestroyCharacter(_leftCharacter);
            _battleCleanupService.DestroyCharacter(_rightCharacter);
            _battleCleanupService.ClearAllProjectiles();

            _leftCharacter = null;
            _rightCharacter = null;
        }
    }
}