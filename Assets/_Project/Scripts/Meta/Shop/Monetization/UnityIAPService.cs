using UnityEngine.Purchasing;
using UnityEngine;
using System;
using Zenject;

namespace FirstProject.Meta.Shop
{
    public class UnityIAPService : IInitializable, IIAPService, IStoreListener
    {
        private IStoreController _storeController;
        private Action<bool> _onPurchaseComplete;
        private bool _isPurchaseInProgress;

        private const string NO_ADS_ID = "com.game.noads";
        private const string COIN_PACK_ID = "com.game.coinpack";
        private const string STARTER_PACK_ID = "com.game.starterpack";

        public void Initialize()
        {
            var module = StandardPurchasingModule.Instance();

            module.useFakeStoreAlways = true;
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(NO_ADS_ID, ProductType.NonConsumable);
            builder.AddProduct(COIN_PACK_ID, ProductType.Consumable);
            builder.AddProduct(STARTER_PACK_ID, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void BuyProduct(ProductId productId, Action<bool> onComplete)
        {
            if (_storeController == null) 
            {
                Debug.LogError("IAP not initialized"); 
                onComplete?.Invoke(false); 
                return; 
            }

            if (_isPurchaseInProgress)
            {
                Debug.Log("Purchase is already in progress!");
                onComplete?.Invoke(false);
                return;
            }

            _isPurchaseInProgress = true;
            _onPurchaseComplete = onComplete;

            switch (productId)
            {
                case ProductId.NoAds:
                    _storeController.InitiatePurchase(NO_ADS_ID);
                    break;
                case ProductId.CoinPack:
                    _storeController.InitiatePurchase(COIN_PACK_ID);
                    break;
                case ProductId.StarterPack:
                    _storeController.InitiatePurchase(STARTER_PACK_ID);
                    break;
                default:
                    Debug.Log($"Trying to buy unknown position: {productId}");
                    _onPurchaseComplete?.Invoke(false);
                    _onPurchaseComplete = null;
                    _isPurchaseInProgress = false;
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
            bool isSuccess = false;
            string productID = purchaseEvent.purchasedProduct.definition.id;
            switch (productID)
            {
                case NO_ADS_ID: Debug.Log("Bought No Ads");
                    isSuccess = true;
                    break;
                case COIN_PACK_ID: Debug.Log("Bought Coin Pack");
                    isSuccess = true;
                    break;
                case STARTER_PACK_ID: Debug.Log("Bought Starter Pack");
                    isSuccess = true;
                    break;
                default:
                    Debug.Log($"Trying to buy unknown position: {productID}");
                    isSuccess = false;
                    break;
            }
            _onPurchaseComplete?.Invoke(isSuccess);
            _onPurchaseComplete = null;
            _isPurchaseInProgress = false;
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _onPurchaseComplete?.Invoke(false);
            _onPurchaseComplete = null;
            _isPurchaseInProgress = false;
            Debug.LogError($"Purchase failed: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, "No message provided");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"IAP Init Failed: {error} - {message}");
        }
    }
}