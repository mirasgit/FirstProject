using FirstProject.Shop;
using System;
using System.Diagnostics;
using Zenject;

namespace FirstProject.Menu
{
    public class MenuPresenter : IInitializable, IDisposable
    {
        private readonly MenuView _view;
        private readonly ProgressModel _progressModel;
        private readonly IIAPService _iapService;

        public MenuPresenter(MenuView view, ProgressModel progressModel, IIAPService iapService)
        {
            _view = view;
            _progressModel = progressModel;
            _iapService = iapService;
        }

        public void Initialize()
        {
            _view.Subscribe();
            _view.RemoveAdsButtonPressed += OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed += OnPlayButtonPressed;
            if (_progressModel.isAdsRemoved)
            {
                _view.HideRemoveAdsButton();
            }
        }

        public void Dispose()
        {
            _view.Unsubscribe();
            _view.RemoveAdsButtonPressed -= OnRemoveAdsButtonPressed;
            _view.PlayButtonPressed -= OnPlayButtonPressed;
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("BattleSimulation");
        }
    }
}
