using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirstProject.Meta.Shop;
using FirstProject.Meta.Configs;
using FirstProject.Core.Characters;
using Photon.Pun;
using FirstProject.Meta.Multiplayer;

namespace FirstProject.Core.Battle
{
    public class BattleFlow
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleCleanupService _battleCleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;
        private readonly ProgressModel _progressModel;
        private readonly IRemoteConfigService _configService;
        private readonly RemoteCharacterRegistry _remoteCharacterRegistry;
        private readonly MatchStateService _matchStateService;

        private Character _leftCharacter;
        private Character _rightCharacter;
        public BattleResult LastWinner { get; private set; }
        public BattleState State { get; private set; }

        public event Action<BattleResult> WinnerDecided;
        public event Action StartScreenShowed;
        public event Action BattleStarted;
        public event Action RoundFinished;
        public event Action ShopOpened;
        public event Action ShopClosed;

        public BattleFlow(
           CharacterFactory characterFactory,
           BattleCleanupService cleanupService,
           Transform leftSpawnPoint,
           Transform rightSpawnPoint, 
           ProgressModel model, 
           IRemoteConfigService configService,
           RemoteCharacterRegistry remoteCharacterRegistry, MatchStateService matchStateService)
        {
            _characterFactory = characterFactory;
            _battleCleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
            _progressModel = model;
            _configService = configService;
            _remoteCharacterRegistry = remoteCharacterRegistry;
            _matchStateService = matchStateService;
        }

        public async UniTask LoadCharactersAsync(CancellationToken token)
        {
            await _characterFactory.LoadCharactersAsync(token);
        }

        public void ClaimReward()
        {
            _progressModel.AddCoins(_configService.Data.AdsConfig.RewardedAdReward);
        }

        public void HideShopScreen()
        {
            ShopClosed?.Invoke();
        }

        public void ShowShopScreen()
        {
            ShopOpened?.Invoke();
        }

        public void ShowStartScreen()
        {
            State = BattleState.StartScreen;
            StartScreenShowed?.Invoke();
        }

        public async UniTask StartBattleAsync(CancellationToken token)
        {
            if (!PhotonNetwork.InRoom)
            {
                await StartOffline(token);
            }
            else
            {
                await StartOnline(token);
            }
        }

        public void RestartBattle(CancellationToken token)
        {
            StartBattleAsync(token).Forget();
        }

        private async UniTask StartOnline(CancellationToken token)
        {
            if (State == BattleState.Loading || State == BattleState.Running)
            {
                return;
            }

            State = BattleState.Loading;

            ClearBattle();

            _remoteCharacterRegistry.RemoteCharacterSpawned += OnRemoteCharacterSpawned;
            _matchStateService.OpponentDisconnected += OnOpponentDisconnected;

            if (PhotonNetwork.IsMasterClient)
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
        }

        private async UniTask StartOffline(CancellationToken token)
        {

            if (State == BattleState.Loading || State == BattleState.Running)
            {
                return;
            }

            State = BattleState.Loading;

            ClearBattle();

            try
            {
                token.ThrowIfCancellationRequested();

                SpawnCharacters();

                State = BattleState.Running;

                BattleStarted?.Invoke();
            }
            catch (OperationCanceledException)
            {
                State = BattleState.StartScreen;
                throw;
            }
            catch (Exception ex)
            {
                State = BattleState.StartScreen;
                Debug.LogException(ex);
                ClearBattle();
            }
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

        private void OnOpponentDisconnected()
        {
            if (State != BattleState.Running)
            {
                return;
            }

            if (_leftCharacter != null)
            {
                _leftCharacter.DisallowToFight();
            }

            if (_rightCharacter != null)
            {
                _rightCharacter.DisallowToFight();
            }

            LastWinner = PhotonNetwork.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon;

            State = BattleState.Finished;

            WinnerDecided?.Invoke(LastWinner);
            _progressModel.AddCoins(_configService.Data.BattleSettings.WinReward);

            RoundFinished();
        }

        private void OnRemoteCharacterSpawned(Character character)
        {
            if (PhotonNetwork.IsMasterClient)
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

                State = BattleState.Running;
                BattleStarted?.Invoke();
            }
        }

        private void OnCharacterDied()
        {
            if (State != BattleState.Running)
            {
                return;
            }

            if (_leftCharacter == null || _rightCharacter == null)
            {
                return;
            }

            _leftCharacter.DisallowToFight();
            _rightCharacter.DisallowToFight();

            bool isLocalDead = PhotonNetwork.IsMasterClient ? _leftCharacter.IsDead : _rightCharacter.IsDead;

            State = BattleState.Finished;

            if (PhotonNetwork.InRoom)
            {
                if (isLocalDead)
                {
                    LastWinner = PhotonNetwork.IsMasterClient ? BattleResult.RightWon : BattleResult.LeftWon;
                    WinnerDecided?.Invoke(LastWinner);
                }
                else
                {
                    LastWinner = PhotonNetwork.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon;
                    WinnerDecided?.Invoke(LastWinner);
                    _progressModel.AddCoins(_configService.Data.BattleSettings.WinReward);
                }
            }
            else
            {
                if (_leftCharacter.IsDead)
                {
                    LastWinner = BattleResult.RightWon;
                    WinnerDecided?.Invoke(LastWinner);
                }
                else if (_rightCharacter.IsDead)
                {
                    LastWinner = BattleResult.LeftWon;
                    WinnerDecided?.Invoke(LastWinner);
                    _progressModel.AddCoins(_configService.Data.BattleSettings.WinReward);
                }
            }

            RoundFinished?.Invoke();
        }

        private void ClearBattle()
        {
            _remoteCharacterRegistry.RemoteCharacterSpawned -= OnRemoteCharacterSpawned;
            _matchStateService.OpponentDisconnected -= OnOpponentDisconnected;
            _remoteCharacterRegistry.Clear();

            if (_rightCharacter != null)
            {
                _rightCharacter.Died -= OnCharacterDied;
            }

            if (_leftCharacter != null)
            {
                _leftCharacter.Died -= OnCharacterDied;
            }

            DestroyCharacterSafe(_leftCharacter);
            DestroyCharacterSafe(_rightCharacter);

            _battleCleanupService.ClearAllTemporaryObjects();

            _leftCharacter = null;
            _rightCharacter = null;
        }

        private void DestroyCharacterSafe(Character character)
        {
            if (character == null) return;

            PhotonView view = character.GetComponent<PhotonView>();

            if (view != null && PhotonNetwork.InRoom)
            {
                if (view.IsMine)
                {
                    PhotonNetwork.Destroy(character.gameObject);
                }
            }
            else
            {
                _battleCleanupService.DestroyCharacter(character);
            }
        }
    }
}