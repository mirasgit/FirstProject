using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifeTime = 1f;
    [SerializeField] private float _moveSpeed = 1f;

    private float _timer;

    public void SetText(string textValue)
    {
        _text.text = textValue;
    }

    private void Update()
    {
        transform.position += Vector3.down * (_moveSpeed * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer >= _lifeTime)
        {
            Destroy(gameObject);
        }
    }
}