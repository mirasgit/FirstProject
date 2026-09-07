using UnityEngine.Purchasing;
using UnityEngine;
using System;
using Zenject;

namespace FirstProject.Shop
{
    public class UnityIAPService : IInitializable, IIAPService, IStoreListener
    {
        private IStoreController _storeController;
        private Action<bool> _onPurchaseComplete;

        public void Initialize()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct("com.game.noads", ProductType.NonConsumable);
            builder.AddProduct("com.game.coinpack", ProductType.Consumable);
            builder.AddProduct("com.game.starterpack", ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void BuyProduct(ProductId productId, Action <bool> onComplete)
        {
            _onPurchaseComplete = onComplete;

            switch (productId)
            {
                case ProductId.NoAds:
                    _storeController.InitiatePurchase("com.game.noads");
                    break;
                case ProductId.CoinPack:
                    _storeController.InitiatePurchase("com.game.coinpack");
                    break;
                case ProductId.StarterPack:
                    _storeController.InitiatePurchase("com.game.starterpack");
                    break;
                default:
                    Debug.Log($"Trying to buy unknown position: {productId}");
                    break;
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            Debug.Log("IAP Initialized");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (purchaseEvent.purchasedProduct.definition.id == "com.game.noads")
            {
                _onPurchaseComplete?.Invoke(true);
            }
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _onPurchaseComplete?.Invoke(false);
            Debug.LogError($"Purchase failed: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {

        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {

        }
    }
}