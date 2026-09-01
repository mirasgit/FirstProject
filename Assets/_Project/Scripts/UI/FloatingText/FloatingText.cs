using FirstProject.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace FirstProject.UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private float _lifeTime = 1f;
        private float _moveSpeed = 1f;

        private FloatingTextRegistry _floatingTextRegistry;
        private RemoteConfigService _configService;
        
        public void SetText(string textValue)
        {
            _text.text = textValue;
        }

        [Inject]
        public void Construct(FloatingTextRegistry floatingTextRegistry, RemoteConfigService configService)
        {
            _configService = configService;
            _floatingTextRegistry = floatingTextRegistry;
            _floatingTextRegistry.Register(this);
        }

        private void Start()
        {
            _lifeTime = _configService.Data.FloatingTextSettings.LifeSpanInSeconds;
            _moveSpeed = _configService.Data.FloatingTextSettings.MoveSpeed;
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.position += Vector3.down * (_moveSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_floatingTextRegistry == null)
            {
                return;
            }
            _floatingTextRegistry.Unregister(this);
        }
    }
}