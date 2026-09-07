using Cysharp.Threading.Tasks;
using FirstProject.MatchupConfigs;
using UnityEngine;

namespace FirstProject.Configs
{
    public interface IRemoteConfigService
    {
        GameConfigData Data { get; }

        UniTask FetchConfigAsync();

        CharacterSettings GetCharacterConfig(CharacterClass characterClass  );


    }
}