using System.Collections;
using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.CharacterEffect
{
    public class StunEffect : CharacterApplicableEffect
    {
        private readonly float _duration;

        public StunEffect(float duration)
            : base(EffectType.Stun, "Stunned")
        {
            _duration = duration;
        }

        public override IEnumerator Run(Character character)
        {
            character.EnablePlayStun(true);

            yield return new WaitForSeconds(_duration);

            character.EnablePlayStun(false);
        }
    }
}