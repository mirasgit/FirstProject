using UnityEngine;

public class BattleCompositionRoot : MonoBehaviour
{
    [SerializeField] private CharacterFactory _characterFactory;
    [SerializeField] private BattleView _battleView;
    [SerializeField] private BattleCleanupService _battleCleanupService;
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private BattleFlow _battleFlow;
    [SerializeField] private ProjectileRegistry _projectileRegistry;


    private void Awake()
    {
        _battleFlow.Initialize(_characterFactory, _battleView, _battleCleanupService, _leftSpawnPoint, _rightSpawnPoint, _projectileRegistry);
    }

    public BattleFlow GetBattleFlow()
    {
        return _battleFlow;
    }
}
