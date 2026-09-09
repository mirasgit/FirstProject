using UnityEngine;
using Zenject;
using FirstProject.Menu.UI;

namespace FirstProject.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private SaveConflictView _conflictView;
        [SerializeField] private Transform _uiCanvas;
        public override void InstallBindings()
        {
            Container.Bind<MenuView>().FromComponentInNewPrefab(_menuView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MenuEntryPoint>().AsSingle().NonLazy();
            Container.Bind<SaveConflictView>().FromComponentInNewPrefab(_conflictView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<SaveConflictResolver>().AsSingle();
        }
    }
}