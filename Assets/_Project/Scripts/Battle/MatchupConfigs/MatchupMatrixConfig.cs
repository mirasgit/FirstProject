using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.MatchupConfigs
{
    [System.Serializable]
    public struct ClassMatchup
    {
        public CharacterClass Attacker;
        public CharacterClass Defender;
        public float DamageMultiplier;
    }

    [CreateAssetMenu(fileName ="New Matchup Matrix", menuName ="Battle/Matchup Matrix")]
    public class MatchupMatrixConfig : ScriptableObject
    {
        [SerializeField] private ClassMatchup[] _matchups;

        public float GetMultiplier(CharacterClass attacker, CharacterClass defender)
        {
            foreach (var matchup in _matchups)
            {
                if (matchup.Attacker == attacker && matchup.Defender == defender)
                {
                    return matchup.DamageMultiplier;
                }
            }
            return 1.0f;
        }
    }
}