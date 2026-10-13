using System.Collections;
using UnityEngine;

namespace FirstProject.Core.Characters.Effects
{
    public class WeaknessEffect : CharacterApplicableEffect
    {
        public readonly float _coefficient;
        public readonly WaitForSeconds _weaknessTime;
        public readonly float _duration;

        public WeaknessEffect(float duration, float coefficient)
            : base(EffectType.Weakness, "Weakness")
        {
            _duration = duration;
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