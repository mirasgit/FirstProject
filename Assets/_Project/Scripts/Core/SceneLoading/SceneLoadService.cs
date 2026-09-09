namespace FirstProject.Core
{
    public class SceneLoadService : ISceneLoadService
    {
        public void LoadScene(SceneName sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
        }
    }
}
