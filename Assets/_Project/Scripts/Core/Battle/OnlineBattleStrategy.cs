using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using FirstProject.Core.Characters;
using FirstProject.Meta.Multiplayer;

namespace FirstProject.Core.Battle
{
    public class OnlineBattleStrategy : BaseBattleStrategy
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleCleanupService _cleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;
        private readonly IMultiplayerService _multiplayerService;
        private readonly INetworkSpawner _networkSpawner;
        private readonly RemoteCharacterRegistry _remoteCharacterRegistry;
        private readonly MatchStateService _matchStateService;

        public OnlineBattleStrategy(
            CharacterFactory characterFactory,
            BattleCleanupService cleanupService,
            Transform leftSpawnPoint,
            Transform rightSpawnPoint,
            IMultiplayerService multiplayerService,
            INetworkSpawner networkSpawner,
            RemoteCharacterRegistry remoteCharacterRegistry,
            MatchStateService matchStateService)
        {
            _characterFactory = characterFactory;
            _cleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
            _multiplayerService = multiplayerService;
            _networkSpawner = networkSpawner;
            _remoteCharacterRegistry = remoteCharacterRegistry;
            _matchStateService = matchStateService;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            _remoteCharacterRegistry.RemoteCharacterSpawned += OnRemoteCharacterSpawned;
            _matchStateService.OpponentDisconnected += OnOpponentDisconnected;

            if (_multiplayerService.IsMasterClient)
            {
                _leftCharacter = _characterFactory.SpawnRandomNetworkCharacter(_leftSpawnPoint, true);
                _leftCharacter.Died += OnCharacterDied;
            }
            else
            {
                _rightCharacter = _characterFactory.SpawnRandomNetworkCharacter(_rightSpawnPoint, false);
                _rightCharacter.Died += OnCharacterDied;
            }

            if (_remoteCharacterRegistry.CurrentRemoteCharacter != null)
            {
                OnRemoteCharacterSpawned(_remoteCharacterRegistry.CurrentRemoteCharacter);
            }

            await UniTask.CompletedTask;
        }

        private void OnRemoteCharacterSpawned(Character character)
        {
            if (_multiplayerService.IsMasterClient)
            {
                _rightCharacter = character;
            }
            else
            {
                _leftCharacter = character;
            }

            character.Died -= OnCharacterDied;
            character.Died += OnCharacterDied;

            if (_leftCharacter != null && _rightCharacter != null)
            {
                _leftCharacter.AllowToFight();
                _rightCharacter.AllowToFight();
                InvokeBattleStarted();
            }
        }

        private void OnOpponentDisconnected()
        {
            if (_leftCharacter != null)
            {
                _leftCharacter.DisallowToFight();
            }
            if (_rightCharacter != null)
            {
                _rightCharacter.DisallowToFight();
            }

            LastWinner = _multiplayerService.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon;
            InvokeWinnerDecided(LastWinner);
        }

        protected override void OnCharacterDied()
        {
            base.OnCharacterDied();

            bool isLocalDead = _multiplayerService.IsMasterClient ? _leftCharacter.IsDead : _rightCharacter.IsDead;
            LastWinner = isLocalDead
                ? (_multiplayerService.IsMasterClient ? BattleResult.RightWon : BattleResult.LeftWon)
                : (_multiplayerService.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon);

            InvokeWinnerDecided(LastWinner);
        }

        public override void Clear()
        {
            _remoteCharacterRegistry.RemoteCharacterSpawned -= OnRemoteCharacterSpawned;
            _matchStateService.OpponentDisconnected -= OnOpponentDisconnected;
            _remoteCharacterRegistry.Clear();

            if (_leftCharacter != null)
            {
                _leftCharacter.Died -= OnCharacterDied;
            } 
            if (_rightCharacter != null)
            {
                _rightCharacter.Died -= OnCharacterDied;
            } 

            DestroyCharacterSafe(_leftCharacter);
            DestroyCharacterSafe(_rightCharacter);

            _cleanupService.ClearAllTemporaryObjects();

            _leftCharacter = null;
            _rightCharacter = null;
        }

        private void DestroyCharacterSafe(Character character)
        {
            if (character == null) return;

            _networkSpawner.Destroy(character.gameObject);
        }
    }
}