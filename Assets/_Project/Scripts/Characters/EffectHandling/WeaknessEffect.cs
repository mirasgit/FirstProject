using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class WeaknessEffect : CharacterApplicableEffect
    {
        private readonly float _coefficient;
        private readonly WaitForSeconds _weaknessTime;

        public WeaknessEffect(float duration, float coefficient)
            : base(EffectType.Weakness, "Weakness")
        {
            _coefficient = coefficient;
            _weaknessTime = new WaitForSeconds(duration);
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            context.AddDamageModifier(-_coefficient);

            yield return _weaknessTime;

            CancelEffect(context);
        }

        public override void CancelEffect(CharacterEffectContext context)
        {
            context.RemoveDamageModifier(-_coefficient);
        }
    }
}