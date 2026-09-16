using System;

namespace FirstProject.Meta.Shop
{
    public interface IIAPService
    {

        void BuyProduct(ProductId id, Action<bool> onComplete);
    }
}
