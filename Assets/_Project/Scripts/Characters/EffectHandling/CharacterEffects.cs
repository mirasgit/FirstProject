using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstProject.CharacterEffect
{
    public class CharacterEffects : MonoBehaviour
    {
        private Character _character;

        private List<CharacterApplicableEffect> _effects = new List<CharacterApplicableEffect>();

        public void Initialize(Character character)
        {
            _character = character;
        }

        public void Apply(CharacterApplicableEffect effect)
        {
            if (HasEffect(effect.Type))
                return;

            _effects.Add(effect);

            _character.CharacterUI.ShowEffect(effect.Name);

            StartCoroutine(RunEffect(effect));
        }

        private IEnumerator RunEffect(CharacterApplicableEffect effect)
        {
            yield return effect.Run(_character);

            _effects.Remove(effect);

            _character.CharacterUI.HideEffect();
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