using TMPro;
using UnityEngine;
using UnityEngine.UI;

   public class CharacterUI : MonoBehaviour
   {
       [SerializeField] private Character _character;
       [SerializeField] private Image _healthFillImage;
       [SerializeField] private TMP_Text _effectText;
       [SerializeField] private Transform _floatingTextSpawnPoint;
       [SerializeField] private FloatingText _floatingTextPrefab;

       private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;

            if (_effectText != null)
                _effectText.text = "";
            if (_character != null)
                _character.HealthChanged += OnHealthChanged;

    }
    private void Start()
    {
        if (_character != null)
            OnHealthChanged(_character.Stats._currentHealth, _character.Stats._maxHealth);
    }
    private void Update()
        {
            if (_character == null) return;

            FaceCamera();
        }
        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            UpdateHealthBar(currentHealth, maxHealth);
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
            textInstance.SetText(damage.ToString("0"));
        }

        public void ShowEffect(string effectName)
        {
            if (_effectText == null) return;
            _effectText.text = effectName;
        }

        public void HideEffect()
        {
            if (_effectText == null) return;
            _effectText.text = "";
        }
    private void OnDestroy()
    {
        if (_character != null)
            _character.HealthChanged -= OnHealthChanged;
    }
}