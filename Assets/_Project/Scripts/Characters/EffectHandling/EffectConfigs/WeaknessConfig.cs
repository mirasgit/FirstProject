using UnityEngine;

namespace FirstProject.CharacterEffect.Configs
{
    [CreateAssetMenu(fileName = "NewWeaknessCfg", menuName = "Effects/Weakness Effect")]
    public class WeaknessConfig : EffectConfig
    {
        [Header("Weakness Settings")]
        [SerializeField] private float _coefficient;
        [SerializeField] private float _duration;

        public override CharacterApplicableEffect CreateEffect()
        {
            return new WeaknessEffect(_duration, _coefficient);
        }
    }
}

