using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainScript
{
    public class LevelComplete : MonoBehaviour
    {
        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Next Scene Loaded!");
        }

        public void QuitApp()
        {
            Application.Quit();
            Debug.Log("Application Quitted!");
        }
    }
}
