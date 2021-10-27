using UnityEngine;
using UnityEngine.UI;

namespace MainScript
{
    public class PauseMenu : MonoBehaviour
    {

        public static bool GameIsPaused = false;
        public GameObject pausemenuUI;
        public GameObject pausebutton;
        public GameObject joystickUI;
        public GameObject moremenu;
        public GameManager gameManager;
        public Text levelConfigure;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }

                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            Debug.Log("Game resumed!");
            pausemenuUI.SetActive(false);
            pausebutton.SetActive(true);
            joystickUI.SetActive(true);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void Pause()
        {
            Debug.Log("Game paused!");
            pausemenuUI.SetActive(true);
            pausebutton.SetActive(false);
            joystickUI.SetActive(false);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void Restart()
        {
            gameManager.RestartGame();
            GameIsPaused = false;
            Time.timeScale = 1f;
        }

        public void MoreMenu()
        {
            pausemenuUI.SetActive(false);
            moremenu.SetActive(true);
        }

        public void LevelConfigure()
        {
            gameManager.LevelChange();
            Time.timeScale = 1f;
        }

        public void NextLevel()
        {
            gameManager.NextLevel();
        }

        public void ReturnHome()
        {
            Time.timeScale = 1f;
            gameManager.ReturnHome();
        }

        public void Back()
        {
            moremenu.SetActive(false);
            pausemenuUI.SetActive(true);
        }
    }
}
