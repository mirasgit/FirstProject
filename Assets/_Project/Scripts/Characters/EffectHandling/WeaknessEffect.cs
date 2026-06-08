using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class WeaknessEffect : CharacterApplicableEffect
    {
        private readonly float _duration;
        private readonly float _coefficient;
        private float _oldDamage;

        public WeaknessEffect(float duration, float coefficient)
            : base(EffectType.Weakness, "Weakness")
        {
            _duration = duration;
            _coefficient = coefficient;
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            _oldDamage = context.GetCurrentDamage();

            float newDamage = _oldDamage - _oldDamage * _coefficient;

            context.ChangeDamageTo(newDamage);

            yield return new WaitForSeconds(_duration);

            context.ChangeDamageTo(_oldDamage);
        }
    }
}