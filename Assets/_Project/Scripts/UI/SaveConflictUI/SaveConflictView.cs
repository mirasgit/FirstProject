using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.Menu.UI
{
    public class SaveConflictView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _localButton;
        [SerializeField] private Button _cloudButton;
        [SerializeField] private TMP_Text _localInfoText;
        [SerializeField] private TMP_Text _cloudInfoText;

        public event Action LocalButtonPressed;
        public event Action CloudButtonPressed;

        private void Awake()
        {
            Hide();
        }

        public void Subscribe()
        {
            _localButton.onClick.AddListener(OnLocalButtonClicked);
            _cloudButton.onClick.AddListener(OnCloudButtonClicked);
        }

        public void Unsubscribe()
        {
            _localButton.onClick.RemoveListener(OnLocalButtonClicked);
            _cloudButton.onClick.RemoveListener(OnCloudButtonClicked);

        }

        public void Hide()
        {
            _panel.SetActive(false);
        }

        public void Show(string localInfo, string cloudInfo)
        {
            _localInfoText.text = localInfo;
            _cloudInfoText.text = cloudInfo;
            _panel.SetActive(true);
        }

        private void OnLocalButtonClicked()
        {
            LocalButtonPressed?.Invoke();
        }

        private void OnCloudButtonClicked()
        {
            CloudButtonPressed?.Invoke();
        }

    }
}