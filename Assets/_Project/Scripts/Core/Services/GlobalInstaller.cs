using FirstProject.Core.SaveSystem;
using FirstProject.Core.SceneLoading;
using FirstProject.Meta.Ads;
using FirstProject.Meta.Analytics;
using FirstProject.Meta.Authentication;
using FirstProject.Meta.Configs;
using FirstProject.Meta.Shop;
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
            Container.BindInterfacesAndSelfTo<LocalSaveProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<CloudSaveProvider>().AsSingle();
            Container.Bind<UGSAuthService>().AsSingle();
            Container.Bind<IResourceProvider>().To<AddressablesProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle().NonLazy();
        }
    }
}