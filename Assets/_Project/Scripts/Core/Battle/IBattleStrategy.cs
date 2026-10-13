using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace FirstProject.Core.Battle
{
    public interface IBattleStrategy
    {
        BattleResult LastWinner { get; }
        event Action BattleStarted;
        event Action<BattleResult> WinnerDecided;

        UniTask StartAsync(CancellationToken token);
        void Clear();
    }
}