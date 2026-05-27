using System.Collections;
using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.CharacterEffect
{
    public class StunEffect : CharacterApplicableEffect
    {
        private float _duration;

        public StunEffect(float duration)
            : base(EffectType.Stun, "Stunned")
        {
            _duration = duration;
        }

        public override IEnumerator Run(Character character)
        {
            character.CharAnimator.PlayStun();

            yield return new WaitForSeconds(_duration);

            character.CharAnimator.StopPlayStun();
        }
    }
}