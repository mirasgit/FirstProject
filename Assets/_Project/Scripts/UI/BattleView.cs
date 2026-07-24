using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.UI
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winnerText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _restartButton;

        public event Action StartButtonPressed;
        public event Action RestartButtonPressed;

        public void Subscribe()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        public void ShowStartScreen()
        {
            _startPanel.SetActive(true);
            _winPanel.SetActive(false);
        }

        public void ShowBattleScreen()
        {
            _startPanel.SetActive(false);
            _winPanel.SetActive(false);
        }

        public void ShowWinner(string winnerText)
        {
            _winnerText.text = winnerText;
            _winPanel.SetActive(true);
        }

        private void OnStartButtonClicked()
        {
            StartButtonPressed?.Invoke();
        }

        private void OnRestartButtonClicked()
        {
            RestartButtonPressed?.Invoke();
        }

        public void Unsubscribe()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
    }
}