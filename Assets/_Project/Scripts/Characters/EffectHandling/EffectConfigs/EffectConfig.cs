using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public abstract class EffectConfig : ScriptableObject
    {
        public abstract CharacterApplicableEffect CreateEffect();
    }
}
