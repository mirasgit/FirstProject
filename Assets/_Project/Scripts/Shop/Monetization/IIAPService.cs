using System;

namespace FirstProject.Shop
{
    public interface IIAPService
    {

        void BuyProduct(ProductId id, Action<bool> onComplete);
    }
}
