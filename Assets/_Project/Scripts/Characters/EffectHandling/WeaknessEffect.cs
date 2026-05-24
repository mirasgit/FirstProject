using System.Collections;
using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.CharacterEffect
{
    public class WeaknessEffect : CharacterApplicableEffect
    {
        private float _duration;
        private float _coefficient;
        private float _oldDamage;

        public WeaknessEffect(float duration, float coefficient)
            : base(EffectType.Weakness, "Weakness")
        {
            _duration = duration;
            _coefficient = coefficient;
        }

        public override IEnumerator Run(Character character)
        {
            _oldDamage = character.Stats.CurrentDamage;

            float newDamage = _oldDamage - _oldDamage * _coefficient;

            character.Stats.ChangeDamage(newDamage);

            yield return new WaitForSeconds(_duration);

            character.Stats.ChangeDamage(_oldDamage);
        }
    }
}