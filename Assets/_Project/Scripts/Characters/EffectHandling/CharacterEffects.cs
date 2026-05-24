using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstProject.Characters;

namespace FirstProject.CharacterEffect
{
    public class CharacterEffects : MonoBehaviour
    {
        private Character _character;

        private List<CharacterApplicableEffect> _effects = new List<CharacterApplicableEffect>();

        public event Action<CharacterApplicableEffect> EffectApplied;
        public event Action EffectEnded;
        public void Initialize(Character character)
        {
            _character = character;
        }

        public void Apply(CharacterApplicableEffect effect)
        {
            if (HasEffect(effect.Type))
                return;

            _effects.Add(effect);
            EffectApplied?.Invoke(effect);
            StartCoroutine(RunEffect(effect));
        }

        private IEnumerator RunEffect(CharacterApplicableEffect effect)
        {
            yield return effect.Run(_character);
            EffectEnded?.Invoke();
            _effects.Remove(effect);
        }

        public bool HasEffect(EffectType type)
        {
            foreach (CharacterApplicableEffect effect in _effects)
            {
                if (effect.Type == type) 
                    return true;
            }
            return false;
        }
    }
}