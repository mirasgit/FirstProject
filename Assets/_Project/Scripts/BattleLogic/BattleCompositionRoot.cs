using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using FirstProject.UI;
using FirstProject.Projectiles;
using FirstProject.Characters;

namespace FirstProject.Battle
{
    public class BattleCompositionRoot : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;
        [SerializeField] private List<Character> _characterPrefabs;
        private BattleEntryPoint _battleEntryPoint;
        
        private void Awake()
        {
            _battleEntryPoint = new BattleEntryPoint(_startButton, _restartButton, Compose());
            _battleEntryPoint.Subscribe();
            _battleEntryPoint.Start();
        }

        private BattleFlow Compose()
        {
            ProjectileRegistry projectileRegistry = new();
            BattleCleanupService cleanupService = new(projectileRegistry);
            CharacterFactory characterFactory = new(projectileRegistry, _characterPrefabs);

            return new BattleFlow(
                characterFactory,
                _battleView,
                cleanupService,
                _leftSpawnPoint,
                _rightSpawnPoint
            );
        }

        private void OnDestroy()
        {
            _battleEntryPoint.Unsubscribe();
        }
    }
}