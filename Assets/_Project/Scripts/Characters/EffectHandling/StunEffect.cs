using System.Collections;
using UnityEngine;

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

        public override IEnumerator Run(CharacterEffectContext context)
        {
            context.EnablePlayStun(true);

            yield return new WaitForSeconds(_duration);

            context.EnablePlayStun(false);
        }
    }
}