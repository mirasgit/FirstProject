using System.Collections;
using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters;

public class PoisonEffect : CharacterApplicableEffect
{
    private float _duration;
    private float _interval;
    private float _tickDamage;
    public PoisonEffect(float duration, float interval, float tickDamage) : base(EffectType.Poison, "Poisoned")
    {
        _duration = duration;
        _interval = interval;
        _tickDamage = tickDamage;
    }

    public override IEnumerator Run(Character character)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            character.TakeDamage(_tickDamage);

            yield return new WaitForSeconds(_interval);

            elapsed += _interval;
        }
    }

}
