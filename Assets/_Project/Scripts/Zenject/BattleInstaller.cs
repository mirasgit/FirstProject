using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FirstProject.UI;
using FirstProject.Characters;
using FirstProject.Projectiles;

namespace FirstProject.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [SerializeField] private List<Character> _characterPrefabs;

        public override void InstallBindings()
        {
            Container.Bind<ProjectileRegistry>().AsSingle();
            Container.Bind<BattleCleanupService>().AsSingle();
            Container.Bind<CharacterFactory>().AsSingle().WithArguments(_characterPrefabs);
            Container.Bind<BattleView>().AsSingle().WithArguments(_startButton, _restartButton);
            Container.Bind<BattleFlow>().AsSingle().WithArguments(_leftSpawnPoint, _rightSpawnPoint);
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle().WithArguments(_battleView).NonLazy();
            Container.BindInterfacesAndSelfTo<BattleEntryPoint>().AsSingle().NonLazy();
            Container.Bind<FloatingTextRegistry>().AsSingle();

        }
    }
}
