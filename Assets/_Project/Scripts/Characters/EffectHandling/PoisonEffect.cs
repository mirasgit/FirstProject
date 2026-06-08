using System.Collections;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class PoisonEffect : CharacterApplicableEffect
    {
        private readonly float _duration;
        private readonly float _interval;
        private readonly float _tickDamage;
        public PoisonEffect(float duration, float interval, float tickDamage) : base(EffectType.Poison, "Poisoned")
        {
            _duration = duration;
            _interval = interval;
            _tickDamage = tickDamage;
        }

        public override IEnumerator Run(CharacterEffectContext context)
        {
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                context.TakeDamage(_tickDamage);

                yield return new WaitForSeconds(_interval);

                elapsed += _interval;
            }
        }

    }
}