using UnityEngine;
using Zenject;
using FirstProject.UI;
using FirstProject.Projectiles;
using FirstProject.Shop;
using FirstProject.Analytics;
using FirstProject.Battle.UI;
using FirstProject.Shop.UI;
using FirstProject.MatchupConfigs;
using FirstProject.Core;
using FirstProject.Ads;

namespace FirstProject.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [SerializeField] private Transform _uiCanvas;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private MatchupMatrixConfig _matchupMatrix;
        [SerializeField] private UpgradeConfig _upgradeConfig;
        [SerializeField] private int _winReward;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectileRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<FloatingTextRegistry>().AsSingle();

            Container.Bind<BattleCleanupService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterFactory>().AsSingle();
            Container.Bind<BattleView>().FromComponentInNewPrefab(_battleView).UnderTransform(_uiCanvas).AsSingle();
            Container.Bind<BattleFlow>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint, _winReward);
            Container.BindInterfacesAndSelfTo<BattleEntryPoint>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle().NonLazy();
            Container.Bind<ProjectileFactory>().AsSingle();
            Container.Bind<FloatingTextFactory>().AsSingle();
            Container.Bind<ShopView>().FromComponentInNewPrefab(_shopView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopPresenter>().AsSingle().NonLazy();
            Container.Bind<ISaveService>().To<SaveService>().AsSingle();
            Container.Bind<ProgressModel>().AsSingle();
            Container.BindInstance(_matchupMatrix).AsSingle();
            Container.BindInstance(_upgradeConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattleAnalyticsTracker>().AsSingle().NonLazy();
            Container.Bind<IResourceProvider>().To<AddressablesProvider>().AsSingle();  
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattleAdsTracker>().AsSingle().NonLazy();
        }
    }
}
