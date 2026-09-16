using FirstProject.Core.Battle;
using Zenject;

namespace FirstProject.UI
{
    public class FloatingTextFactory : BaseFactory<FloatingText>
    {
        public FloatingTextFactory(IInstantiator instantiator) : base(instantiator)
        {

        }

    }

}