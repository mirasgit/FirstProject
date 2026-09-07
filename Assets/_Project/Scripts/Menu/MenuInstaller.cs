using FirstProject.Analytics;
using FirstProject.Configs; 
using FirstProject.Shop;
using UnityEngine;
using Zenject;

namespace FirstProject.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private Transform _uiCanvas;
        public override void InstallBindings()
        {
            Container.Bind<MenuView>().FromComponentInNewPrefab(_menuView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuPresenter>().AsSingle().NonLazy();
            Container.Bind<ProgressModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityIAPService>().AsSingle();
            Container.Bind<ISaveService>().To<SaveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RemoteConfigService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MenuEntryPoint>().AsSingle().NonLazy();
        }
    }
}