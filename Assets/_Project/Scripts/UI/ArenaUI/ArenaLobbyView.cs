using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.UI.Multiplayer
{
    public class ArenaLobbyView : MonoBehaviour
    {
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private TMP_InputField _input;

        public event Action ConnectButtonPressed;
        public event Action CreateRoomButtonPressed;

        public string GetInputText()
        {
            return _input.text;
        }

        public void Subscribe()
        {
            _connectButton.onClick.AddListener(OnConnectButtonClicked);
            _createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
        }

        public void Unsubscribe()
        {
            _connectButton.onClick.RemoveListener(OnConnectButtonClicked);
            _createRoomButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
        }

        private void OnConnectButtonClicked()
        {
            ConnectButtonPressed?.Invoke();
            Debug.Log("Connect pressed VIEW");
        }

        private void OnCreateRoomButtonClicked()
        {
            CreateRoomButtonPressed.Invoke();
            Debug.Log("Create pressed VIEW");
        }

        public void AllowButtonsInteraction(bool allow)
        {
            _connectButton.interactable = allow;
            _createRoomButton.interactable = allow;
            _input.interactable = allow;
        }
    }
}