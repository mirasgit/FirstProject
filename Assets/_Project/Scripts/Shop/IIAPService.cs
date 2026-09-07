using System;

namespace FirstProject.Shop
{
    public interface IIAPService
    {
        void Initialize();

        void BuyProduct(ProductId id, Action<bool> onComplete);
    }
}
