using FirstProject.Core.Projectiles;
using FirstProject.Meta.Ads;
using FirstProject.Meta.Analytics;
using FirstProject.UI.Battle;
using FirstProject.UI;
using FirstProject.UI.Shop;
using UnityEngine;
using Zenject;

namespace FirstProject.Core.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [SerializeField] private Transform _uiCanvas;
        [SerializeField] private ShopView _shopView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectileRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<FloatingTextRegistry>().AsSingle();

            Container.Bind<BattleCleanupService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterFactory>().AsSingle();
            Container.Bind<BattleView>().FromComponentInNewPrefab(_battleView).UnderTransform(_uiCanvas).AsSingle();
            Container.Bind<BattleFlow>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint);
            Container.BindInterfacesAndSelfTo<BattleEntryPoint>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle().NonLazy();
            Container.Bind<ProjectileFactory>().AsSingle();
            Container.Bind<FloatingTextFactory>().AsSingle();
            Container.Bind<ShopView>().FromComponentInNewPrefab(_shopView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattleAnalyticsTracker>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattleAdsTracker>().AsSingle().NonLazy();
        }
    }
}
