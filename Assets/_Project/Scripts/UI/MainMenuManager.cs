using UnityEngine;

namespace FirstProject.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            Debug.Log("Exit Button has been pressed.");
#else
            Application.Quit();
#endif
        }
    }
}
