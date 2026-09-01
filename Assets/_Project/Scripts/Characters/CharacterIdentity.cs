using FirstProject.MatchupConfigs;
using UnityEngine;

namespace FirstProject.Characters
{
    public class CharacterIdentity : MonoBehaviour
    {
        [field: SerializeField] public CharacterClass MyClass { get; private set; }
    }
}