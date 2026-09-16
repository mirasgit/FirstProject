
using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Core.SaveSystem
{
    public interface ISaveProvider 
    {
        UniTask<SaveData> LoadAsync(CancellationToken token);
        UniTask SaveAsync(SaveData data, CancellationToken token);
    }
}