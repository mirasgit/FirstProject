using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Core.SaveSystem
{
    public interface ISaveService
    {
        UniTask<SaveData> LoadAsync(ISaveConflictResolver resolver, CancellationToken token = default);

        UniTask SaveAsync(SaveData data, CancellationToken token = default);
    }
}
