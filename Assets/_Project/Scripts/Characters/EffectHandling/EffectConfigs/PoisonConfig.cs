using UnityEngine;

namespace FirstProject.CharacterEffect.Configs
{
    [CreateAssetMenu(fileName = "NewPoisonCfg", menuName = "Effects/Poison Effect")]
    public class PoisonConfig : EffectConfig
    {
        [Header("Poison Settings")]
        [SerializeField] private float _duration;
        [SerializeField] private float _interval;
        [SerializeField] private float _tickDamage;

        public override CharacterApplicableEffect CreateEffect()
        {
            return new PoisonEffect(_duration, _interval, _tickDamage);
        }
    }
}

