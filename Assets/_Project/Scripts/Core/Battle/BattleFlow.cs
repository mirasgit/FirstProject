using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirstProject.Meta.Shop;
using FirstProject.Meta.Configs;
using FirstProject.Meta.Multiplayer;

namespace FirstProject.Core.Battle
{
    public class BattleFlow
    {
        private readonly OfflineBattleStrategy _offlineStrategy;
        private readonly OnlineBattleStrategy _onlineStrategy;
        private readonly IMultiplayerService _multiplayerService;
        private readonly CharacterFactory _characterFactory;
        private readonly ProgressModel _progressModel;
        private readonly IRemoteConfigService _configService;

        private IBattleStrategy _currentStrategy;

        public BattleResult LastWinner => _currentStrategy?.LastWinner ?? BattleResult.None;
        public BattleState State { get; private set; }

        public event Action<BattleResult> WinnerDecided;
        public event Action StartScreenShowed;
        public event Action BattleStarted;
        public event Action RoundFinished;
        public event Action ShopOpened;
        public event Action ShopClosed;

        public BattleFlow(
            OfflineBattleStrategy offlineStrategy,
            OnlineBattleStrategy onlineStrategy,
            IMultiplayerService multiplayerService,
            CharacterFactory characterFactory,
            ProgressModel progressModel,
            IRemoteConfigService configService)
        {
            _offlineStrategy = offlineStrategy;
            _onlineStrategy = onlineStrategy;
            _multiplayerService = multiplayerService;
            _characterFactory = characterFactory;
            _progressModel = progressModel;
            _configService = configService;
        }

        public async UniTask LoadCharactersAsync(CancellationToken token)
        {
            await _characterFactory.LoadCharactersAsync(token);
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

            _currentStrategy = _multiplayerService.InRoom ? (IBattleStrategy)_onlineStrategy : _offlineStrategy;

            _currentStrategy.BattleStarted += OnBattleStarted;
            _currentStrategy.WinnerDecided += OnWinnerDecided;

            await _currentStrategy.StartAsync(token);
        }

        public void RestartBattle(CancellationToken token)
        {
            StartBattleAsync(token).Forget();
        }

        private void ClearBattle()
        {
            if (_currentStrategy == null)
            {
                return;
            }

            _currentStrategy.BattleStarted -= OnBattleStarted;
            _currentStrategy.WinnerDecided -= OnWinnerDecided;
            _currentStrategy.Clear();
        }

        private void OnBattleStarted()
        {
            State = BattleState.Running;
            BattleStarted?.Invoke();
        }

        private void OnWinnerDecided(BattleResult winner)
        {
            State = BattleState.Finished;

            BattleResult localSide = _multiplayerService.InRoom
                ? (_multiplayerService.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon)
                : BattleResult.LeftWon;

            if (winner == localSide)
            {
                _progressModel.AddCoins(_configService.Data.BattleSettings.WinReward);
            }

            WinnerDecided?.Invoke(winner);
            RoundFinished?.Invoke();
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
    }
}