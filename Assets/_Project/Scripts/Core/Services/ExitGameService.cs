using UnityEngine;

namespace FirstProject.Meta.Menu
{
    public class ExitGameService
    {
        public void ExitGame()
        {

#if UNITY_EDITOR
            Debug.Log("Exit button has been pressed");
#else
            Application.Quit();
#endif

        }
    }
}