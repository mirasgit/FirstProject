using System;
using UnityEngine;
using FirstProject.Characters;
using FirstProject.Shop;

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
        private BattleResult _lastWinner;
        private BattleState _previousState;
        public BattleState State { get; private set; }

        public event Action<BattleResult> WhoWon;
        public event Action StartScreenShowed;
        public event Action BattleStarted;
        public event Action RoundFinished;
        public event Action ShopScreenShowed;

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

        public void AddCoins(int coins)
        {
            _model.AddCoins(coins);
        }

        public void HideShopScreen()
        {
           if (_previousState == BattleState.StartScreen)
            {
                ShowStartScreen();
            }
           else if (_previousState == BattleState.Finished)
            {
                State = BattleState.Finished;
                WhoWon?.Invoke(_lastWinner);
                RoundFinished?.Invoke();
            }
        }

        public void ShowShopScreen()
        {
            _previousState = State;
            State = BattleState.Shop;
            ShopScreenShowed?.Invoke();
        }

        public void ShowStartScreen()
        {
            _previousState = State;
            State = BattleState.StartScreen;
            StartScreenShowed?.Invoke();
        }

        public void StartBattle()
        {
            ClearBattle();
            _previousState = State;
            State = BattleState.Running;
            SpawnCharacters();
            BattleStarted?.Invoke();
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
                _lastWinner = BattleResult.RightWon;
                WhoWon?.Invoke(_lastWinner);
                State = BattleState.Finished;
            }
            else if (_rightCharacter.IsDead)
            {
                _lastWinner = BattleResult.LeftWon;
                WhoWon?.Invoke(_lastWinner);
                State = BattleState.Finished;
                AddCoins(_winReward);
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