using System;
using UnityEngine;
using FirstProject.Characters;
using FirstProject.Shop;
using FirstProject.Analytics;
using Cysharp.Threading.Tasks;

namespace FirstProject.Battle
{
    public class BattleFlow
    {
        private readonly CharacterFactory _characterFactory;
        private readonly BattleCleanupService _battleCleanupService;
        private readonly Transform _leftSpawnPoint;
        private readonly Transform _rightSpawnPoint;
        private readonly int _winReward;
        private readonly ProgressModel _model;
        private Character _leftCharacter;
        private Character _rightCharacter;
        private BattleState _previousState;
        public BattleResult LastWinner { get; private set; }
        public BattleState State { get; private set; }

        public event Action WinScreenShowed;
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
           Transform rightSpawnPoint, ProgressModel model, int winReward)
        {
            _characterFactory = characterFactory;
            _battleCleanupService = cleanupService;
            _leftSpawnPoint = leftSpawnPoint;
            _rightSpawnPoint = rightSpawnPoint;
            _model = model;
            _winReward = winReward;
        }

        public void ClaimReward()
        {
            AddCoins(_winReward);
        }
        public void AddCoins(int coins)
        {
            _model.AddCoins(coins);
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
            _previousState = State;
            State = BattleState.StartScreen;
            StartScreenShowed?.Invoke();
        }

        public async UniTask StartBattleAsync()
        {
            ClearBattle();
            _previousState = State;
            State = BattleState.Running;

            await _characterFactory.LoadCharactersAsync();

            SpawnCharacters();
            BattleStarted?.Invoke();
        }

        public void RestartBattle()
        {
            StartBattleAsync().Forget();
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

            _previousState = State;

            if (_leftCharacter.IsDead)
            {
                LastWinner = BattleResult.RightWon;
                WinnerDecided?.Invoke(LastWinner);
                State = BattleState.Finished;
                WinScreenShowed?.Invoke();
            }
            else if (_rightCharacter.IsDead)
            {
                LastWinner = BattleResult.LeftWon;
                WinnerDecided?.Invoke(LastWinner);
                State = BattleState.Finished;
                AddCoins(_winReward);
                WinScreenShowed?.Invoke();
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