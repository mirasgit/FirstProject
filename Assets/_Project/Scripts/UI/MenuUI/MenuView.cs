using System;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.UI.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _removeAdsButton;

        public event Action PlayButtonPressed;
        public event Action RemoveAdsButtonPressed;

        public void SetInteractable(bool isInteractable)
        {
            _playButton.interactable = isInteractable;
            _removeAdsButton.interactable = isInteractable;
        }

        public void HideRemoveAdsButton()
        {
            _removeAdsButton.gameObject.SetActive(false);
        }

        public void Subscribe()
        {
            _playButton.onClick.AddListener(OnPlayButtonPressed);
            _removeAdsButton.onClick.AddListener(OnRemoveAdsButtonPressed);
        }

        public void Unsubscribe()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonPressed);
            _removeAdsButton.onClick.RemoveListener(OnRemoveAdsButtonPressed);
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