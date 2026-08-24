using UnityEngine;

namespace FirstProject.CharacterEffect.Configs
{
    [CreateAssetMenu(fileName = "NewStunCfg", menuName = "Effects/Stun Effect")]
    public class StunConfig : EffectConfig
    {
        [Header("Stun Settings")]
        [SerializeField] private float _duration;

        public override CharacterApplicableEffect CreateEffect()
        {
            return new StunEffect(_duration);
        }
    }
}

