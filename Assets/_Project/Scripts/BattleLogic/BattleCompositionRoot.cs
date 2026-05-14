using UnityEngine;

public class BattleCompositionRoot : MonoBehaviour
{
    [SerializeField] private CharacterFactory _characterFactory;
    [SerializeField] private BattleView _battleView;
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;


    public BattleFlow Compose()
    {
        ProjectileRegistry projectileRegistry = new ProjectileRegistry();
        BattleCleanupService cleanupService = new BattleCleanupService(projectileRegistry);

        _characterFactory.Initialize(projectileRegistry);

        return new BattleFlow(
            _characterFactory,
            _battleView,
            cleanupService,
            _leftSpawnPoint,
            _rightSpawnPoint
        );
    }
}
