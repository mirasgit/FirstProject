using FirstProject.Core.SceneLoading;
using FirstProject.Meta.Multiplayer;
using FirstProject.Meta.Shop;
using Photon.Pun;
using System;
using Zenject;

namespace FirstProject.UI.Menu
{
    public class MenuPresenter : IInitializable, IDisposable
    {
        private readonly MenuView _view;
        private readonly ProgressModel _progressModel;
        private readonly IIAPService _iapService;
        private readonly ISceneLoadService _sceneLoader;
        private readonly PhotonConnectionService _connectionService;
        private readonly ExitGameService _exitGameService;

        public MenuPresenter(MenuView view,
            ProgressModel progressModel,
            IIAPService iapService,
            ISceneLoadService sceneLoader,
            PhotonConnectionService connectionService,
            ExitGameService exitGameService)
        {
            _view = view;
            _progressModel = progressModel;
            _iapService = iapService;
            _sceneLoader = sceneLoader;
            _connectionService = connectionService;
            _exitGameService = exitGameService;
        }

        public void Initialize()
        {
            _view.SetInteractable(false);
            _view.Subscribe();
            _view.RemoveAdsButtonPressed += OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed += OnPlayButtonPressed;
            _view.ArenaButtonPressed += OnArenaButtonPressed;
            _view.CreateButtonPressed += OnCreateRoomButtonPressed;
            _view.ConnectButtonPressed += OnConnectButtonPressed;
            _view.OkayButtonPressed += OnOkayButtonPressed;
            _view.ExitButtonPressed += OnExitButtonPressed;
            _progressModel.DataChanged += OnDataChanged;

            _connectionService.ConnectedToMaster += OnConnectedToMaster;
            _connectionService.JoinedRoom += OnJoinedRoom;
            _connectionService.RoomOperationFailed += OnRoomOperationFailed;
                
            if (_progressModel.IsInitialized)
            {
                OnDataChanged();
            }

            if (PhotonNetwork.IsConnectedAndReady)
            {
                OnConnectedToMaster();
            }
        }

        public void Dispose()
        {
            _view.Unsubscribe();
            _view.RemoveAdsButtonPressed -= OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed -= OnPlayButtonPressed;
            _view.ArenaButtonPressed -= OnArenaButtonPressed;
            _view.CreateButtonPressed -= OnCreateRoomButtonPressed;
            _view.ConnectButtonPressed -= OnConnectButtonPressed;
            _view.OkayButtonPressed -= OnOkayButtonPressed;
            _view.ExitButtonPressed -= OnExitButtonPressed;
            _progressModel.DataChanged -= OnDataChanged;

            _connectionService.ConnectedToMaster -= OnConnectedToMaster;
            _connectionService.JoinedRoom -= OnJoinedRoom;
            _connectionService.RoomOperationFailed -= OnRoomOperationFailed;
        }

        private void OnDataChanged()
        {
            if (_progressModel.IsInitialized)
            {
                _view.SetInteractable(true);
            }

            if (_progressModel.IsAdsRemoved)
            {
                _view.HideRemoveAdsButton();
            }
        }

        private void OnExitButtonPressed()
        {
            _exitGameService.ExitGame();
        }

        private void OnOkayButtonPressed()
        {
            _view.ShowArenaMenu();
        }

        private void OnRoomOperationFailed(string error)
        {
            _view.ShowLoading(false);
            _view.ShowError(error);
        }

        private void OnJoinedRoom()
        {
            _view.ShowLoading(false);
        }

        private void OnConnectedToMaster()
        {
            _view.SetInteractableArena(true);
        }

        private void OnCreateRoomButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(_view.GetInputText()))
            {
                return;
            }

            _view.ShowLoading(true);
            _connectionService.CreateRoom(_view.GetInputText());
        }

        private void OnConnectButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(_view.GetInputText()))
            {
                return;
            }

            _view.ShowLoading(true);
            _connectionService.ConnectToRoom(_view.GetInputText());
        }


        private void OnRemoveAdsButtonPressed()
        {
            _iapService.BuyProduct(ProductId.NoAds, isSuccess =>
            {
                if (isSuccess)
                {

                    _view.HideRemoveAdsButton();
                    _progressModel.RemoveAds();
                }
            });
        }

        private void OnArenaButtonPressed()
        {
            _view.ShowArenaMenu();
        }

        private void OnPlayButtonPressed()
        {
            _sceneLoader.LoadScene(SceneName.BattleSimulation);
        }
    }
}
