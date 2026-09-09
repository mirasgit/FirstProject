using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Core
{
    public interface ISaveConflictResolver
    {
        UniTask<SaveData> ResolveConflictAsync(SaveData localData, SaveData cloudData, CancellationToken token = default);
    }
}
