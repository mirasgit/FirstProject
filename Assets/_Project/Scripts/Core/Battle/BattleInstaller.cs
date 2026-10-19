using FirstProject.Core.Projectiles;
using FirstProject.Meta.Ads;
using FirstProject.Meta.Analytics;
using FirstProject.UI.Battle;
using FirstProject.UI;
using FirstProject.UI.Shop;
using UnityEngine;
using Zenject;
using FirstProject.Meta.Multiplayer;

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
            Container.Bind<BattleFlow>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleEntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle();
            Container.Bind<ProjectileFactory>().AsSingle();
            Container.Bind<FloatingTextFactory>().AsSingle();
            Container.Bind<ShopView>().FromComponentInNewPrefab(_shopView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleAnalyticsTracker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleAdsTracker>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkCharacterSpawner>().AsSingle(); 
            Container.Bind<RemoteCharacterRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<PhotonNetworkSpawnerWrapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<PhotonMultiplayerService>().AsSingle();
            Container.Bind<OfflineBattleStrategy>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint);
            Container.Bind<OnlineBattleStrategy>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint);
        }
    }
}
