using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class StunEffect : CharacterApplicableEffect
    {
        private readonly WaitForSeconds _duration;

        public StunEffect(float duration)
            : base(EffectType.Stun, "Stunned")
        {
            _duration = new WaitForSeconds(duration);
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            context.EnablePlayStun(true);

            yield return _duration;

            context.EnablePlayStun(false);
        }
    }
}