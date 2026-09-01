using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.Battle.UI
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

        public event Action StartButtonPressed;
        public event Action RestartButtonPressed;
        public event Action ExitButtonPressed;
        public event Action RewardButtonPressed;

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
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            Debug.Log("Exit Button has been pressed.");
#else
            Application.Quit();
#endif
        }

        public void HideRewardButton()
        {
            _rewardButton.gameObject.SetActive(false);
        }

        public void OnRewardButtonClicked()
        {
            RewardButtonPressed?.Invoke();
        }

        public void ShowStartScreen()
        {
            _startPanel.SetActive(true);
            _winPanel.SetActive(false);
        }

        public void HideScreens()
        {
            _startPanel.SetActive(false);
            _winPanel.SetActive(false);
        }

        public void ShowWinner(string winnerText, bool showRewardButton)
        {
            _winnerText.text = winnerText;
            _winPanel.SetActive(true);
            _rewardButton.gameObject.SetActive(showRewardButton);
        }
            
        public void Unsubscribe()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
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