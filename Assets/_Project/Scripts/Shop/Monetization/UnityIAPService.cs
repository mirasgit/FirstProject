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

        private const string NO_ADS_ID = "com.game.noads";
        private const string COIN_PACK_ID = "com.game.coinpack";
        private const string STARTER_PACK_ID = "com.game.starterpack";

        public void Initialize()
        {
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
            string productID = purchaseEvent.purchasedProduct.definition.id;
            switch (productID)
            {
                case NO_ADS_ID: Debug.Log("Bought No Ads"); 
                    break;
                case COIN_PACK_ID: Debug.Log("Bought Coin Pack");
                    break;
                case STARTER_PACK_ID: Debug.Log("Bought Starter Pack");
                    break;
                default:
                    Debug.LogError($"Unkown product: {productID}");
                    break;
            }
            _onPurchaseComplete?.Invoke(true);
            _onPurchaseComplete = null;
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _onPurchaseComplete?.Invoke(false);
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