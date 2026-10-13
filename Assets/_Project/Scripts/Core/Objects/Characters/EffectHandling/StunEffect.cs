using System.Collections;
using UnityEngine;

namespace FirstProject.Core.Characters.Effects
{
    public class StunEffect : CharacterApplicableEffect
    {
        public readonly WaitForSeconds _durationWait;
        public readonly float _duration;

        public StunEffect(float duration)
            : base(EffectType.Stun, "Stunned")
        {
            _duration = duration;
            _durationWait = new WaitForSeconds(_duration);
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            context.EnablePlayStun(true);

            yield return _durationWait;

            CancelEffect(context);
        }

        public override void CancelEffect(CharacterEffectContext context)
        {
            context.EnablePlayStun(false);
        }
    }
}