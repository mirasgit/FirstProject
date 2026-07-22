using FirstProject.Projectiles;
using TMPro;
using UnityEngine;
using Zenject;

namespace FirstProject.UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _lifeTime = 1f;
        [SerializeField] private float _moveSpeed = 1f;

        private FloatingTextRegistry _floatingTextRegistry;

        public void SetText(string textValue)
        {
            _text.text = textValue;
        }

        [Inject]
        public void Construct(FloatingTextRegistry floatingTextRegistry)
        {
            _floatingTextRegistry = floatingTextRegistry;
            _floatingTextRegistry.Register(this);
        }

        private void Start()
        {
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