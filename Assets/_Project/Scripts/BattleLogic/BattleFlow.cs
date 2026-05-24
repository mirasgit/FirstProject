using UnityEngine;
using FirstProject.UI;
using FirstProject.Characters;

namespace FirstProject.Battle
{
    public class BattleFlow
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
            _battleView.ShowStartScreen();
        }
        public void Tick(float deltaTime)
        {
            CheckForDeath();
        }

        private void CheckForDeath()
        {
            if (_battleStarted == false) return;

            if (_battleFinished) return;

            if (_leftCharacter == null || _rightCharacter == null) return;

            if (_leftCharacter.IsDead)
            {
                _battleFinished = true;
                _battleView.ShowWinner("Right won");

            }
            else if (_rightCharacter.IsDead)
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
}