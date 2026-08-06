using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FirstProject.UI;
using FirstProject.Characters;
using FirstProject.Projectiles;
using FirstProject.Shop;

namespace FirstProject.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [SerializeField] private Transform _uiCanvas;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private List<Character> _characterPrefabs;
        [SerializeField] private MatchupMatrixConfig _matchupMatrix;
        [SerializeField] private UpgradeConfig _upgradeConfig;
            public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectileRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<FloatingTextRegistry>().AsSingle();

            Container.Bind<BattleCleanupService>().AsSingle();
            Container.Bind<CharacterFactory>().AsSingle().WithArguments(_characterPrefabs);
            Container.Bind<BattleView>().FromComponentInNewPrefab(_battleView).UnderTransform(_uiCanvas).AsSingle();
            Container.Bind<BattleFlow>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint);
            Container.BindInterfacesAndSelfTo<BattleEntryPoint>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle().NonLazy();
            Container.Bind<ProjectileFactory>().AsSingle();
            Container.Bind<FloatingTextFactory>().AsSingle();
            Container.Bind<ShopView>().FromComponentInNewPrefab(_shopView).UnderTransform(_uiCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopPresenter>().AsSingle().NonLazy();
            Container.Bind<SaveService>().AsSingle().NonLazy();
            Container.BindInstance(_matchupMatrix).AsSingle();
            Container.BindInstance(_upgradeConfig).AsSingle();
        }
    }
}
