using FirstProject.CharacterEffect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FirstProject.UI
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private TMP_Text _effectText;
        [SerializeField] private Transform _floatingTextSpawnPoint;
        [SerializeField] private FloatingText _floatingTextPrefab;

        private const string DAMAGE_TEXT_FORMAT = "0";
        private const string EMPTY_EFFECT_TEXT = "";
        private FloatingTextFactory _factory;

        private void Awake()
        {
            if (_effectText != null)
            {
                _effectText.text = EMPTY_EFFECT_TEXT;
            }
        }

        [Inject]
        public void Construct(FloatingTextFactory factory)
        {
            _factory = factory;
        }

        public void UpdateHealthBar(float fillAmount)
        {
            _healthFillImage.fillAmount = fillAmount;
        }

        public void ShowDamage(float damage)
        {
            if (_floatingTextPrefab == null || _floatingTextSpawnPoint == null)
            {
                return;
            }

            FloatingText textInstance = _factory.Create(_floatingTextPrefab, _floatingTextSpawnPoint.position, Quaternion.identity);
            textInstance.SetText(damage.ToString(DAMAGE_TEXT_FORMAT));

        }

        public void ShowEffect(string effectName)
        {
            if (_effectText == null)
            {
                return;
            }

            _effectText.text = effectName;
        }

        public void HideEffect(CharacterApplicableEffect effect)
        {
            if (_effectText == null)
            {
                return;
            }

            if (_effectText.text != effect.Name)
            {
                return;
            }

            _effectText.text = EMPTY_EFFECT_TEXT;
        }
    }
}