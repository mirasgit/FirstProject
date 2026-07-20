using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class WeaknessEffect : CharacterApplicableEffect
    {
        private readonly float _coefficient;
        private readonly WaitForSeconds _duration;

        public WeaknessEffect(float duration, float coefficient)
            : base(EffectType.Weakness, "Weakness")
        {
            _coefficient = coefficient;
            _duration = new WaitForSeconds(duration);
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            context.AddDamageModifier(-_coefficient);

            yield return _duration;

            context.RemoveDamageModifier(-_coefficient);
        }
    }
}