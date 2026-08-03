using FirstProject.Characters;
using FirstProject.CharacterEffect;
using System.Collections.Generic;
using System.Text;

namespace FirstProject.UI
{
    public class CharacterPresenter
    {
        private readonly CharacterView _view;
        private readonly Character _model;
        private readonly List<string> _effects = new();
        public CharacterPresenter (CharacterView view, Character model)
        {
            _view = view;
            _model = model;
        }

        public void Subscribe()
        {
            _model.HealthChanged += OnHealthChanged;
            _model.DamageTaken += OnDamageTaken;
            _model.EffectApplied += OnEffectApplied;
            _model.EffectEnded += OnEffectEnd;
            _model.Destroyed += OnModelDestroyed;
            OnHealthChanged(_model.CurrentHealth, _model.MaxHealth);
        }

        private void Unsubscribe()
        {
            _model.HealthChanged -= OnHealthChanged;
            _model.DamageTaken -= OnDamageTaken;
            _model.EffectApplied -= OnEffectApplied;
            _model.EffectEnded -= OnEffectEnd;
            _model.Destroyed -= OnModelDestroyed;
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            float normalizedHealth = currentHealth / maxHealth;
            _view.UpdateHealthBar(normalizedHealth);
        }

        private void OnDamageTaken(float damage)
        {
            _view.ShowDamage(damage);
        }

        private void OnEffectApplied(CharacterApplicableEffect effect)
        {
            _effects.Add(effect.Name);
            RefreshEffectView();
        }

        private void OnEffectEnd(CharacterApplicableEffect effect)
        {
            _effects.Remove(effect.Name);
            RefreshEffectView();
        }

        private void RefreshEffectView()
        {
            string combinedText = string.Join(", ", _effects);

            _view.UpdateEffectText(combinedText);
        }

        private void OnModelDestroyed()
        {
            _effects.Clear();
            Unsubscribe();
        }
    }
}