using FirstProject.CharacterEffect;
using Zenject;

namespace FirstProject.Characters
{
    public class CharactersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterEffectContext>().AsSingle();

            Container.BindInterfacesAndSelfTo<Character>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterStats>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterEffects>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterAttack>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterDeath>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterFacing>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterAnimator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterMovement>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterIdentity>().FromComponentInHierarchy().AsSingle();
        }
    }
}