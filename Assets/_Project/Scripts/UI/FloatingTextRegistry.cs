using System.Collections.Generic;
using UnityEngine;
using FirstProject.Battle;

namespace FirstProject.UI
{
    public class FloatingTextRegistry : IClearableRegistry
    {
        private readonly List<FloatingText> _textList = new();
        public void Register(FloatingText text)
        {
            if (text == null)
            {
                return;
            }

            if (_textList.Contains(text))
            {
                return;
            }

            _textList.Add(text);
        }

        public void Unregister(FloatingText text)
        {
            if (text == null)
            {
                return;
            }

            _textList.Remove(text);
        }

        public void ClearAll()
        {
            for (int index = _textList.Count - 1; index >= 0; index--)
            {
                FloatingText text = _textList[index];

                if (text == null)
                {
                    _textList.RemoveAt(index);
                    continue;
                }

                Object.Destroy(text.gameObject);
            }

            _textList.Clear();
        }
    }
}