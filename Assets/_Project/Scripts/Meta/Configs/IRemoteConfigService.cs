using Cysharp.Threading.Tasks;
using FirstProject.MatchupConfigs;
using System.Threading;

namespace FirstProject.Meta.Configs
{
    public interface IRemoteConfigService
    {
        GameConfigData Data { get; }

        UniTask FetchConfigAsync(CancellationToken token);

        CharacterSettings GetCharacterConfig(CharacterClass characterClass);


    }
}