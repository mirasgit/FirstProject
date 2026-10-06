using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.UI.Battle
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winnerText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _readyPanel;

        public event Action StartButtonPressed;
        public event Action RestartButtonPressed;
        public event Action ExitButtonPressed;
        public event Action RewardButtonPressed;
        public event Action ReadyButtonPressed;

        private void Awake()
        {
            HideScreens();
        }

        public void Subscribe()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _rewardButton.onClick.AddListener(OnRewardButtonClicked);
            _readyButton.onClick.AddListener(OnReadyButtonClicked);
        }

        public void Unsubscribe()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
            _readyButton.onClick.RemoveListener(OnReadyButtonClicked);
        }

        public void SetReadyButtonInteractable(bool interactable)
        {
            _readyButton.interactable = interactable;
        }

        public void ShowReadyScreen(bool active)
        {
            _readyPanel.SetActive(active);    
        }

        public void ShowLoadingScreen(bool active)
        {
            _loadingPanel.SetActive(active);
        }

        public void HideRewardButton()
        {
            _rewardButton.gameObject.SetActive(false);
        }

        public void ShowStartScreen(bool active)
        {
            _startPanel.SetActive(active);
            _winPanel.SetActive(!active);
        }

        public void HideScreens()
        {
            _startPanel.SetActive(false);
            _readyPanel.SetActive(false);
            _winPanel.SetActive(false);
        }

        public void ShowWinner(string winnerText, bool showRewardButton)
        {
            _winnerText.text = winnerText;
            _winPanel.SetActive(true);
            _rewardButton.gameObject.SetActive(showRewardButton);
        }

        private void OnReadyButtonClicked()
        {
            ReadyButtonPressed?.Invoke();
        }

        public void OnRewardButtonClicked()
        {
            RewardButtonPressed?.Invoke();
        }

        private void OnStartButtonClicked()
        {
            StartButtonPressed?.Invoke();
        }

        private void OnRestartButtonClicked()
        {
            RestartButtonPressed?.Invoke();
        }

        private void OnExitButtonClicked()
        {
            ExitButtonPressed?.Invoke();
        }
    }
}