using System;
using UnityEngine;
using FirstProject.Characters;
using FirstProject.Shop;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirstProject.Ads;
using FirstProject.Configs;

namespace FirstProject.Battle
{
    public class BattleFlow
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleCleanupService _battleCleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;
        private readonly ProgressModel _model;
        private Character _leftCharacter;
        private Character _rightCharacter;
        private RemoteConfigService _configService;
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
           Transform rightSpawnPoint, ProgressModel model, RemoteConfigService configService)
        {
            _characterFactory = characterFactory;
            _battleCleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
            _model = model;
            _configService = configService;
        }



        public void ClaimReward()
        {
            _model.AddCoins(_configService.Data.AdsConfig.RewardedAdReward);
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
            if (State == BattleState.Loading || State == BattleState.Running)
            {
                return;
            }

            State = BattleState.Loading;

            ClearBattle();
            try
            {

                await _characterFactory.LoadCharactersAsync(token);

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
        }

        public void RestartBattle(CancellationToken token)
        {
            StartBattleAsync(token).Forget();
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

            if (_leftCharacter.IsDead)
            {
                LastWinner = BattleResult.RightWon;
                State = BattleState.Finished;
                WinnerDecided?.Invoke(LastWinner);
            }
            else if (_rightCharacter.IsDead)
            {
                LastWinner = BattleResult.LeftWon;
                State = BattleState.Finished;
                WinnerDecided?.Invoke(LastWinner);
                _model.AddCoins(_configService.Data.BattleSettings.WinReward);
            }

            RoundFinished?.Invoke();
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
            _battleCleanupService.ClearAllTemporaryObjects();

            _leftCharacter = null;
            _rightCharacter = null;
        }
    }
}