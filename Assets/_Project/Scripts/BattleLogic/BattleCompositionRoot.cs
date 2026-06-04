using UnityEngine;
using System.Collections.Generic;
using FirstProject.UI;
using FirstProject.Projectiles;
using FirstProject.Characters;

namespace FirstProject.Battle
{
    public class BattleCompositionRoot : MonoBehaviour
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Transform _leftSpawnPoint;
        [SerializeField] private Transform _rightSpawnPoint;

        [SerializeField] private List<Character> _characterPrefabs;

        public BattleFlow Compose()
        {
            ProjectileRegistry projectileRegistry = new();
            BattleCleanupService cleanupService = new(projectileRegistry);
            CharacterFactory _characterFactory = new(projectileRegistry, _characterPrefabs);

            return new BattleFlow(
                _characterFactory,
                _battleView,
                cleanupService,
                _leftSpawnPoint,
                _rightSpawnPoint
            );
        }
    }
}