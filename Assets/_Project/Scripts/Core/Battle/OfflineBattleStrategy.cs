using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FirstProject.Core.Battle
{
    public class OfflineBattleStrategy : BaseBattleStrategy
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleCleanupService _cleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;

        public OfflineBattleStrategy(
            CharacterFactory characterFactory,
            BattleCleanupService cleanupService,
            Transform leftSpawnPoint,
            Transform rightSpawnPoint)
        {
            _characterFactory = characterFactory;
            _cleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            _leftCharacter = _characterFactory.SpawnRandomCharacter(_leftSpawnPoint, true);
            _rightCharacter = _characterFactory.SpawnRandomCharacter(_rightSpawnPoint, false);

            _leftCharacter.Died += OnCharacterDied;
            _rightCharacter.Died += OnCharacterDied;

            _leftCharacter.AllowToFight();
            _rightCharacter.AllowToFight();

            InvokeBattleStarted();
            await UniTask.CompletedTask;
        }

        protected override void OnCharacterDied()
        {
            base.OnCharacterDied();

            if (_leftCharacter.IsDead)
            {
                LastWinner = BattleResult.RightWon;
            }
            else if (_rightCharacter.IsDead)
            {
                LastWinner = BattleResult.LeftWon;
            }

            InvokeWinnerDecided(LastWinner);
        }

        public override void Clear()
        {
            if (_leftCharacter != null)
            {
                _leftCharacter.Died -= OnCharacterDied;
            }
            if (_rightCharacter != null)
            {
                _rightCharacter.Died -= OnCharacterDied;
            }

            _cleanupService.DestroyCharacter(_leftCharacter);
            _cleanupService.DestroyCharacter(_rightCharacter);
            _cleanupService.ClearAllTemporaryObjects();

            _leftCharacter = null;
            _rightCharacter = null;
        }
    }
}