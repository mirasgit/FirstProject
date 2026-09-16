namespace FirstProject.Core.SceneLoading
{
    public class SceneLoadService : ISceneLoadService
    {
        public void LoadScene(SceneName sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
        }
    }
}
