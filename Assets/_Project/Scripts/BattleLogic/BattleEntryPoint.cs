using System;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.Battle
{
    public class BattleEntryPoint : Zenject.IInitializable, IDisposable
    {
        private readonly Button _startButton;
        private readonly Button _restartButton;
        private readonly BattleFlow _battleFlow;

        public BattleEntryPoint(Button startButton, Button restartButton, BattleFlow battleflow)
        {
            _startButton = startButton;
            _restartButton = restartButton;
            _battleFlow = battleflow;
        }

        public void Initialize()
        {
            _battleFlow.ShowStartScreen();
            Debug.Log("<color=green>ZENJECT РАБОТАЕТ! BattleEntryPoint инициализирован.</color>");
            Subscribe();
        }
        public void Subscribe()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _battleFlow.StartBattle();
        }

        private void OnRestartButtonClicked()
        {
            _battleFlow.RestartBattle();
        }

        public void Dispose()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
    }
}