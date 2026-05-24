using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FirstProject.CharacterEffect;
using FirstProject.Characters;

namespace FirstProject.UI
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private TMP_Text _effectText;
        [SerializeField] private Transform _floatingTextSpawnPoint;
        [SerializeField] private FloatingText _floatingTextPrefab;

        private Camera _mainCamera;
        private Character _character;
        private bool _isInitialized;

        private const string DamageTextFormat = "0";
        private const string EmptyEffectText = "";
        private void Awake()
        {
            _mainCamera = Camera.main;

            if (_effectText != null)
                _effectText.text = "";

        }

        public void Initialize(Character character)
        {
            _character = character;
            _isInitialized = true;
            Subscribe();

            OnHealthChanged(_character.Stats.CurrentHealth, _character.Stats.MaxHealth);
        }
        private void Update()
        {
            if (!_isInitialized)
                return;

            FaceCamera();
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }
        private void Subscribe()
        {
            _character.Stats.HealthChanged += OnHealthChanged;
            _character.Stats.DamageTaken += OnDamageTaken;
            _character.Effects.EffectApplied += OnEffectApplied;
            _character.Effects.EffectEnded += OnEffectEnd;
        }

        private void Unsubscribe()
        {
            if (!_isInitialized || _character == null)
                return;

            _character.Stats.HealthChanged -= OnHealthChanged;
            _character.Stats.DamageTaken -= OnDamageTaken;
            _character.Effects.EffectApplied -= OnEffectApplied;
            _character.Effects.EffectEnded -= OnEffectEnd;
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            UpdateHealthBar(currentHealth, maxHealth);
        }
        private void OnDamageTaken(float damage)
        {
            ShowDamage(damage);
        }
        private void OnEffectApplied(CharacterApplicableEffect effect)
        {
            ShowEffect(effect.Name);
        }
        private void OnEffectEnd()
        {
            HideEffect();
        }
        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            float normalizedHealth = currentHealth / maxHealth;
            _healthFillImage.fillAmount = normalizedHealth;
        }

        private void FaceCamera()
        {
            if (_mainCamera == null) return;
            transform.forward = _mainCamera.transform.forward;
        }

        public void ShowDamage(float damage)
        {
            if (_floatingTextPrefab == null || _floatingTextSpawnPoint == null) return;

            FloatingText textInstance = Instantiate(_floatingTextPrefab, _floatingTextSpawnPoint.position, Quaternion.identity);
            textInstance.SetText(damage.ToString(DamageTextFormat));
        }

        public void ShowEffect(string effectName)
        {
            if (_effectText == null) return;
            _effectText.text = effectName;
        }

        public void HideEffect()
        {
            if (_effectText == null) return;
            _effectText.text = EmptyEffectText;
        }
    }
}