using Zenject;

namespace FirstProject.Battle
{
    public class BattleEntryPoint : IInitializable
    {

        private readonly BattleFlow _battleFlow;

        public BattleEntryPoint(BattleFlow battleflow)
        {
            _battleFlow = battleflow;
        }

        public void Initialize()
        {
            _battleFlow.ShowStartScreen();
        }
    }
}