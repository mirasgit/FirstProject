using TMPro;
using UnityEngine;

namespace FirstProject.UI
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winnerText;
        private BattlePresenter _presenter;

        public void SetPresenter(BattlePresenter presenter)
        {
            _presenter = presenter;
        }

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

        private void OnDestroy()
        {
            _presenter.Unsubscribe();
        }
    }
}