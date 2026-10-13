using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirstProject.Core.Characters;

namespace FirstProject.Core.Battle
{
    public abstract class BaseBattleStrategy : IBattleStrategy
    {
        public BattleResult LastWinner { get; protected set; }

        public event Action BattleStarted;
        public event Action<BattleResult> WinnerDecided;

        protected Character _leftCharacter;
        protected Character _rightCharacter;

        public abstract UniTask StartAsync(CancellationToken token);
        public abstract void Clear();

        protected void InvokeBattleStarted() => BattleStarted?.Invoke();
        protected void InvokeWinnerDecided(BattleResult result) => WinnerDecided?.Invoke(result);

        protected virtual void OnCharacterDied()
        {
            if (_leftCharacter == null || _rightCharacter == null)
            {
                return;
            }

            _leftCharacter.DisallowToFight();
            _rightCharacter.DisallowToFight();
        }
    }
}