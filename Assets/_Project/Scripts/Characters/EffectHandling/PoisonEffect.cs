using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class PoisonEffect : CharacterApplicableEffect
    {
        private readonly float _duration;
        private readonly float _tickDamage;
        private readonly float _interval;
        private readonly WaitForSeconds _tickDelay;
        public PoisonEffect(float duration, float interval, float tickDamage) : base(EffectType.Poison, "Poisoned")
        {
            _duration = duration;
            _tickDamage = tickDamage;
            _interval = interval;
            _tickDelay = new WaitForSeconds(interval);
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                context.TakeDamage(_tickDamage);

                yield return _tickDelay;

                elapsed += _interval;
            }
        }

    }
}