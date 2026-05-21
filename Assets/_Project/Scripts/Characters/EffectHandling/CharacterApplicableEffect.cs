using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class CharacterApplicableEffect
    {
        public EffectType Type { get; private set; }
        public string Name { get; private set; }

        public CharacterApplicableEffect(EffectType type, string name)
        {
            Type = type;
            Name = name;
        }

        public virtual IEnumerator Run(Character character)
        {
            yield break;
        }
    }
}