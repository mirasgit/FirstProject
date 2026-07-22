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
        public event Action EffectEnded;
        
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
            return _effects.Exists(effect => effect.Type == type);
        }

        private IEnumerator RunEffect(CharacterApplicableEffect effect)
        {
            yield return effect.Run(_context);
            EffectEnded?.Invoke();
            _effects.Remove(effect);
        }

        private void OnCharacterDied()
        {
            StopAllCoroutines();
            EffectEnded?.Invoke();
            _effects.Clear();
        }

        private void OnDestroy()
        {
            _killable.Died -= OnCharacterDied;
        }
    }
}