using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.UI.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _removeAdsButton;
        [SerializeField] private Button _arenaButton;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _arenaMenuPanel;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _createButton;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _errorPanel;
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private Button _okayButton;
        [SerializeField] private Button _exitButton;
 
        public event Action PlayButtonPressed;
        public event Action RemoveAdsButtonPressed;
        public event Action ArenaButtonPressed;
        public event Action ConnectButtonPressed;
        public event Action CreateButtonPressed;
        public event Action OkayButtonPressed;
        public event Action ExitButtonPressed;

        public void ShowError(string error)
        {
            _errorPanel.SetActive(true);

            _errorText.text = error;
        }

        public void ShowLoading(bool enable)
        {
            _loadingPanel.SetActive(enable);
        }

        public string GetInputText()
        {
            return _input.text;
        }

        public void ShowArenaMenu()
        {
            _mainMenuPanel.SetActive(false);
            _loadingPanel.SetActive(false);
            _errorPanel.SetActive(false);
            _arenaMenuPanel.SetActive(true);
        }

        public void SetInteractableArena(bool allow)
        {
            _connectButton.interactable = allow;
            _createButton.interactable = allow;
            _input.interactable = allow;
        }

        public void SetInteractable(bool isInteractable)
        {
            _playButton.interactable = isInteractable;
            _removeAdsButton.interactable = isInteractable;
            _arenaButton.interactable = isInteractable;
        }

        public void HideRemoveAdsButton()
        {
            _removeAdsButton.gameObject.SetActive(false);
        }

        public void Subscribe()
        {
            _playButton.onClick.AddListener(OnPlayButtonPressed);
            _removeAdsButton.onClick.AddListener(OnRemoveAdsButtonPressed);
            _arenaButton.onClick.AddListener(OnArenaButtonPressed);
            _createButton.onClick.AddListener(OnCreateButtonPressed);   
            _connectButton.onClick.AddListener(OnConnectButtonPressed);
            _okayButton.onClick.AddListener(OnOkayButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        public void Unsubscribe()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonPressed);
            _removeAdsButton.onClick.RemoveListener(OnRemoveAdsButtonPressed);
            _arenaButton.onClick.RemoveListener(OnArenaButtonPressed);
            _createButton.onClick.RemoveListener(OnCreateButtonPressed);
            _connectButton.onClick.RemoveListener(OnConnectButtonPressed);
            _okayButton.onClick.RemoveListener(OnOkayButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }

        private void OnExitButtonPressed()
        {
            ExitButtonPressed?.Invoke();
        }

        private void OnOkayButtonPressed()
        {
            OkayButtonPressed?.Invoke();
        }

        private void OnCreateButtonPressed()
        {
            CreateButtonPressed?.Invoke();
        }

        private void OnConnectButtonPressed()
        {
            ConnectButtonPressed?.Invoke();
        }

        private void OnArenaButtonPressed()
        {
            ArenaButtonPressed?.Invoke();
        }

        private void OnPlayButtonPressed()
        {
            PlayButtonPressed?.Invoke();
        }

        private void OnRemoveAdsButtonPressed()
        {
            RemoveAdsButtonPressed?.Invoke();
        }
    }
}