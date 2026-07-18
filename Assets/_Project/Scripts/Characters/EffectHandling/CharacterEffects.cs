using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FirstProject.CharacterEffect
{
    public class CharacterEffects : MonoBehaviour
    {
        private CharacterEffectContext _context;
        private readonly List<CharacterApplicableEffect> _effects = new();

        public event Action<CharacterApplicableEffect> EffectApplied;
        public event Action EffectEnded;

        [Inject]
        public void Construct(CharacterEffectContext context)
        {
            _context = context;
        }

        public void Apply(CharacterApplicableEffect effect)
        {
            if (HasEffect(effect.Type))
            {
                return;
            }

            _effects.Add(effect);
            EffectApplied?.Invoke(effect);
            StartCoroutine(RunEffect(effect));
        }

        private IEnumerator RunEffect(CharacterApplicableEffect effect)
        {
            yield return effect.Run(_context);
            EffectEnded?.Invoke();
            _effects.Remove(effect);
        }

        public bool HasEffect(EffectType type)
        {
            foreach (CharacterApplicableEffect effect in _effects)
            {
                if (effect.Type == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}