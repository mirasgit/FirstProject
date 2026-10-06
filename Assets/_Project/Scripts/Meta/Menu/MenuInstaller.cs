using UnityEngine;
using Zenject;
using FirstProject.UI.Menu;
using FirstProject.UI.Multiplayer;
using FirstProject.Meta.Multiplayer;

namespace FirstProject.Meta.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private Transform _uiCanvas;
        public override void InstallBindings()
        {
            Container.Bind<MenuView>().FromComponentInNewPrefab(_menuView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MenuEntryPoint>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PhotonConnectionService>().AsSingle();
            Container.Bind<ExitGameService>().AsSingle().NonLazy();
        }
    }
}