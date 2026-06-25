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
            float oldDamage = context.GetCurrentDamage();

            float newDamage = oldDamage - oldDamage * _coefficient;

            context.ChangeDamageTo(newDamage);

            yield return _duration;

            context.ChangeDamageTo(oldDamage);
        }
    }
}