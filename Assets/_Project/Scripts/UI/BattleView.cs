using TMPro;
using UnityEngine;

public class BattleView : MonoBehaviour
{
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winnerText;

        public void ShowStartScreen()
        {
            _startPanel.SetActive(true);
            _winPanel.SetActive(false);
        }

        public void ShowBattleScreen()
        {
            _startPanel.SetActive(false);
            _winPanel.SetActive(false);
        }

        public void ShowWinner(string winnerText)
        {
            _winnerText.text = winnerText;
            _winPanel.SetActive(true);
        }
}