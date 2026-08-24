using UnityEngine;

namespace FirstProject.CharacterEffect.Configs
{
    public abstract class EffectConfig : ScriptableObject
    {
        public abstract CharacterApplicableEffect CreateEffect();
    }
}
