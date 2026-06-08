using TMPro;
using UnityEngine;

namespace FirstProject.UI
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winnerText;

        private const string LeftWonText = "Left Won";
        private const string RightWonText = "Right Won";

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

        public void ShowWinner(BattleResult result)
        {
            switch (result)
            {
                case BattleResult.LeftWon:
                    _winnerText.text = LeftWonText;
                    break;
                case BattleResult.RightWon:
                    _winnerText.text = RightWonText;
                    break;
            }
            _winPanel.SetActive(true);
        }
    }
}