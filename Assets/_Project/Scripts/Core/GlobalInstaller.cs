using FirstProject.Analytics;
using FirstProject.Authentication;
using FirstProject.Configs;
using FirstProject.Shop;
using Zenject;

namespace FirstProject.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveService>().To<SaveManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RemoteConfigService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsService>().AsSingle().NonLazy();
            Container.Bind<ProgressModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityIAPService>().AsSingle();
            Container.Bind<ISceneLoadService>().To<SceneLoadService>().AsSingle();
            Container.Bind<LocalSaveProvider>().AsSingle();
            Container.Bind<CloudSaveProvider>().AsSingle();
            Container.Bind<UGSAuthService>().AsSingle();
        }
    }
}