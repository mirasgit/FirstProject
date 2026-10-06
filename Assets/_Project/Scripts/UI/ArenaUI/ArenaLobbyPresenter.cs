using FirstProject.Meta.Multiplayer;
using System;
using Zenject;

namespace FirstProject.UI.Multiplayer
{
    public class ArenaLobbyPresenter : IInitializable, IDisposable
    {
        private ArenaLobbyView _view;
        private PhotonConnectionService _connectionService;

        public ArenaLobbyPresenter(ArenaLobbyView view, PhotonConnectionService connectionService)
        {
            _view = view;
            _connectionService = connectionService;
        }
        
        public void Initialize()
        {
            _view.Subscribe();
            _view.AllowButtonsInteraction(false);
            _view.ConnectButtonPressed += OnConnectButtonPressed;
            _view.CreateRoomButtonPressed += OnCreateRoomButtonPressed;

            _connectionService.ConnectedToMaster += OnConnectedToMaster;
        }

        public void Dispose()
        {
            _view.Unsubscribe();
            _view.ConnectButtonPressed -= OnConnectButtonPressed;
            _view.CreateRoomButtonPressed -= OnCreateRoomButtonPressed;

            _connectionService.ConnectedToMaster -= OnConnectedToMaster;
        }

        private void OnConnectedToMaster()
        {
            _view.AllowButtonsInteraction(true);
        }

        private void OnCreateRoomButtonPressed()
        {
            _connectionService.CreateRoom(_view.GetInputText());
        }

        private void OnConnectButtonPressed()
        {
            _connectionService.ConnectToRoom(_view.GetInputText());
        }
    }
}
