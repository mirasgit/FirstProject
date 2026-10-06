using System.Collections;
using UnityEngine;

namespace FirstProject.Core.Characters.Effects
{
    public class PoisonEffect : CharacterApplicableEffect
    {
        public readonly float _duration;
        public readonly float _tickDamage;
        public readonly float _interval;
        public readonly WaitForSeconds _tickDelay;

        public PoisonEffect(float duration, float interval, float tickDamage) : base(EffectType.Poison, "Poisoned")
        {
            _duration = duration;
            _tickDamage = tickDamage;
            _interval = interval;
            _tickDelay = new WaitForSeconds(interval);
        }

        public override float[] EffectData()
        {
            return new float[] { _duration, _interval, _tickDamage};
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