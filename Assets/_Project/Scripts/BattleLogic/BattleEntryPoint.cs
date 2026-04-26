using UnityEngine;
using UnityEngine.UI;

public class BattleEntryPoint : MonoBehaviour
{
    [SerializeField] private BattleCompositionRoot _compositionRoot;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;

    private BattleFlow _battleFlow;

    private void Awake()
    {
        _battleFlow = _compositionRoot.GetBattleFlow();
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        _battleFlow.StartBattle();
    }

    private void OnRestartButtonClicked()
    {
        _battleFlow.RestartBattle();
    }
    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }
}
