using System.Collections;
using UnityEngine;
using FirstProject.Characters;

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

        public override IEnumerator Run(Character character)
        {
            _oldDamage = character.GetCurrentDamage();

            float newDamage = _oldDamage - _oldDamage * _coefficient;

            character.ChangeDamageTo(newDamage);

            yield return new WaitForSeconds(_duration);

            character.ChangeDamageTo(_oldDamage);
        }
    }
}