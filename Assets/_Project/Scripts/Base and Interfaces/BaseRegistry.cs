using UnityEngine;
using System.Collections.Generic;

namespace FirstProject.Battle
{
    public class BaseRegistry<T> : IClearableRegistry where T : MonoBehaviour
    {
        private readonly List<T> _listOfElements = new();
        public void Register(T element)
        {
            if (element == null)
            {
                return;
            }

            if (_listOfElements.Contains(element))
            {
                return;
            }

            _listOfElements.Add(element);
        }

        public void Unregister(T element)
        {
            if (element == null)
            {
                return;
            }

            _listOfElements.Remove(element);
        }

        public void ClearAll()
        {
            for (int index = _listOfElements.Count - 1; index >= 0; index--)
            {
                T element = _listOfElements[index];

                if (element == null)
                {
                    _listOfElements.RemoveAt(index);
                    continue;
                }

                Object.Destroy(element.gameObject);
            }

            _listOfElements.Clear();
        }
    }

}

