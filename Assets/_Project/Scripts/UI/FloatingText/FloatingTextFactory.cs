using Zenject;
using FirstProject.Core;

namespace FirstProject.UI
{
    public class FloatingTextFactory : BaseFactory<FloatingText>
    {
        public FloatingTextFactory(IInstantiator instantiator) : base(instantiator)
        {

        }

    }

}