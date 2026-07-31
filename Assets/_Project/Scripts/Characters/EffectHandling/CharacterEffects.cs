using FirstProject.Characters;
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
        private IKillable _killable;
        private readonly List<CharacterApplicableEffect> _effects = new();
        public event Action<CharacterApplicableEffect> EffectApplied;
        public event Action<CharacterApplicableEffect> EffectEnded;
        
        [Inject]
        public void Construct(CharacterEffectContext context, IKillable killable)
        {
            _context = context;
            _killable = killable;
        }

        private void Start()
        {
            _killable.Died += OnCharacterDied;
        }

        public void Apply(CharacterApplicableEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            if (HasEffect(effect.Type))
            {
                return;
            }
            _effects.Add(effect);
            EffectApplied?.Invoke(effect);
            StartCoroutine(RunEffect(effect));
        }

        public bool HasEffect(EffectType type)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Type == type) return true;
            }
            return false;
        }

        private IEnumerator RunEffect(CharacterApplicableEffect effect)
        {
            yield return effect.Run(_context);
            EffectEnded?.Invoke(effect);
            _effects.Remove(effect);
        }

        private void OnCharacterDied()
        {
            StopAllCoroutines();
            foreach (var effect in _effects)
            {
                effect.CancelEffect(_context);
                EffectEnded?.Invoke(effect);
            }
            _effects.Clear();
        }

        private void OnDestroy()
        {
            _killable.Died -= OnCharacterDied;
        }
    }
}