using FirstProject.Core;
using FirstProject.Shop;
using System;
using Zenject;

namespace FirstProject.Menu.UI
{
    public class MenuPresenter : IInitializable, IDisposable
    {
        private readonly MenuView _view;
        private readonly ProgressModel _progressModel;
        private readonly IIAPService _iapService;
        private readonly ISceneLoadService _sceneLoader;

        public MenuPresenter(MenuView view, ProgressModel progressModel, IIAPService iapService, ISceneLoadService sceneLoader)
        {
            _view = view;
            _progressModel = progressModel;
            _iapService = iapService;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _view.Subscribe();
            _view.RemoveAdsButtonPressed += OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed += OnPlayButtonPressed;
            _progressModel.DataChanged += OnDataChanged;
        }

        public void Dispose()
        {
            _view.Unsubscribe();
            _view.RemoveAdsButtonPressed -= OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed -= OnPlayButtonPressed;
            _progressModel.DataChanged -= OnDataChanged;
        }

        private void OnDataChanged()
        {
            if (_progressModel.IsAdsRemoved)
            {
                _view.HideRemoveAdsButton();
            }
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

        private void OnPlayButtonPressed()
        {
            _sceneLoader.LoadScene(SceneName.BattleSimulation);
        }
    }
}
