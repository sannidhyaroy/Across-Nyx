using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace MainScript
{
    public class GameManager : MonoBehaviour
    {

        public bool gameplay = true;
        public bool gamerunning = true;
        public float RestartDelay = 6f;
        public float LevelCompleteDelay = 3f;
        public GameObject levelcompleteui;
        public Text highestscore;
        public GameObject highestscoreUI;
        public GameObject joystickUI;
        public GameObject gyrobuttonUI;
        public GameObject gyrodeactivatebuttonUI;
        public int gyroactive;
        public int LastScene;
        public Animator animator;
        public float HorizontalMove;
        public float VerticalMove;
        public GameObject dialouge;
        public GameObject SkipAndRestartButton;
        public AudioManager audioManager;
        public int[] buildindex = new int[5];
        public PlayFabManager PlayFabManager;

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        private void Start()
        {
            //Enable fog
            //RenderSettings.fog = true;
            gyroactive = PlayerPrefs.GetInt("gyrostatus", 1);
            LastScene = PlayerPrefs.GetInt("LastScene");
            /*audioManager.PlayDelayed("Laugh 1", 10);
            audioManager.PlayDelayed("Laugh 2", 25.8f);*/

            if (gyroactive == 1)
            {
                GyroInputActivate();
            }

            else
            {
                GyroInputDeactivate();
            }
        }

        private void Update()
        {
            PlayerPrefs.SetInt("gyrostatus", gyroactive);
            HorizontalMove = Input.GetAxis("Horizontal");
            //VerticalMove = Input.GetAxis("Vertical");

            if (!FindObjectOfType<GameScript>().enabled)
            {
                VerticalMove = 0;
            }
            else
            {
                VerticalMove = 1;
            }

            if (!gameplay)
            {
                SkipAndRestartButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RestartGame();
                }
            }
            animator.SetFloat("Horizontal motion", HorizontalMove);
            animator.SetFloat("Vertical motion", VerticalMove);
            if (FindObjectOfType<GameScript>().rb.position.y > 2)
            {
                if (GameObject.FindGameObjectsWithTag("Top Plane").Length != 0)
                {
                    if (FindObjectOfType<TopPlane>().UpdateYPosition)
                    {
                        FindObjectOfType<CoinsController>().radius = 20;
                        FindObjectOfType<CoinsController>().PickupSpeed = 30;
                    }
                    else
                    {
                        FindObjectOfType<CoinsController>().radius = 4;
                        FindObjectOfType<CoinsController>().PickupSpeed = 15;
                    }
                }
            }
            if (FindObjectOfType<GameScript>().rb.position.y < 2)
            {
                FindObjectOfType<CoinsController>().radius = 4;
                FindObjectOfType<CoinsController>().PickupSpeed = 15;
            }
            if (FindObjectOfType<GameScript>().rb.position.y < -0.5f)
            {
                FindObjectOfType<Score>().CliffFall();
                //FindObjectOfType<GameManager>().GameOver();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Lose")) // disable fog when Secondary Virtual Camera is activated due to fog bug
            {
                RenderSettings.fog = false;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Win")) // disable fog when Tertiary Virtual Camera is activated due to fog bug
            {
                RenderSettings.fog = false;
            }
            if (GameObject.FindGameObjectsWithTag("Top Plane").Length == 0)
            {
                animator.SetFloat("Vertical Position", FindObjectOfType<GameScript>().rb.position.y);
            }
            else
            {
                if (FindObjectOfType<TopPlane>().UpdateYPosition)
                {
                    animator.SetFloat("Vertical Position", FindObjectOfType<GameScript>().rb.position.y);
                }
            }
        }

        public void GameOver()
        {
            if (gameplay)
            {
                gameplay = false;
                Debug.Log("Game Over!");
                HighestScore();
                joystickUI.SetActive(false);
                FindObjectOfType<PauseMenu>().pausebutton.SetActive(false);

                if (!FindObjectOfType<Score>().cliffFell && FindObjectOfType<CollisionScript>().collided)
                {
                    //animator.CrossFade("Damage 2", 1, -1);
                    animator.SetTrigger("collided");
                    audioManager.Play("waaaa");
                    Invoke("FreezePlayerContraints", 2.5f);
                    audioManager.PlayDelayed("Unfortunately", 3.4f);
                }

                if (FindObjectOfType<Score>().cliffFell)
                {
                    Debug.Log("Collided: " + FindObjectOfType<CollisionScript>().collided + " | Cliff Fell: " + FindObjectOfType<Score>().cliffFell);
                    //animator.CrossFade("Fall", 1, -1);
                    audioManager.Stop("Unfortunately");
                    audioManager.Play("Yes Cheese");
                    dialouge.SetActive(true);
                    FindObjectOfType<GameScript>().rb.constraints = RigidbodyConstraints.FreezeRotationX;
                }
                //RestartGame();
                Invoke("RestartGame", RestartDelay);
            }
            if (!SceneManager.GetActiveScene().name.Equals("Cheat level", System.StringComparison.OrdinalIgnoreCase))
            {
                //FindObjectOfType<CollectablesManager>().AddCoins();
                //FindObjectOfType<CollectablesManager>().AddGems();
                PlayFabManager.SaveProgress();
                PlayFabManager.SendTotalScore(FindObjectOfType<Score>().highestscore);
                Debug.Log("Leaderboard Update Request sent!");
            }
        }

        public void LevelComplete()
        {
            Debug.Log("Level Completed!");
            levelcompleteui.SetActive(true);
            FindObjectOfType<PauseMenu>().pausebutton.SetActive(false);
            joystickUI.SetActive(false);
            gyrobuttonUI.SetActive(false);
            gyrodeactivatebuttonUI.SetActive(false);
            HighestScore();
            if (!SceneManager.GetActiveScene().name.Equals("Cheat level", System.StringComparison.OrdinalIgnoreCase))
            {
                //FindObjectOfType<CollectablesManager>().AddCoins();
                //FindObjectOfType<CollectablesManager>().AddGems();
                PlayFabManager.SaveProgress();
                PlayFabManager.SendTotalScore(FindObjectOfType<Score>().highestscore);
                Debug.Log("Leaderboard Update Request sent!");
            }
        }

        public void LevelCompleteWait()
        {
            //RenderSettings.fog = false; // disable fog
            gameplay = false;
            animator.SetTrigger("win");
            Invoke("LevelComplete", LevelCompleteDelay);
        }

        public void RestartGame()
        {
            Debug.Log("Game restarted!");
            HighestScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            gameplay = true;
        }

        public void RestartGameWait()
        {
            if (!FindObjectOfType<Score>().cliffFell)
            {
                Invoke("RestartGame", RestartDelay / 2);
            }

            else
            {
                FindObjectOfType<Score>().scoretext.text = "You fell off the cliff!";
                audioManager.Play("Yes Cheese");
                Debug.Log("Fall animation activated");
                dialouge.SetActive(true);
                Invoke("RestartGame", RestartDelay / 2);
            }
        }

        public void QuitApp()
        {
            gameplay = false;
            gamerunning = false;
            PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
            Application.Quit();
            Debug.Log("Application Quitted!");
        }

        public void HighestScore()
        {
            highestscore.text = "Highest Score: " + FindObjectOfType<Score>().highestscore.ToString();
            highestscoreUI.SetActive(true);
        }

        public void ActivateCheatScene()
        {
            Debug.Log("Cheat Scene loaded!");
            PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Cheat level");

            /*if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }*/
        }

        public void DeactivateCheatScene()
        {
            Debug.Log("Cheat Scene deactivated!");
            LastScene = PlayerPrefs.GetInt("LastScene");
            SceneManager.LoadScene(LastScene);
        }
        public void ActivateInfiniteLevel()
        {
            Debug.Log("Infinite level loaded!");
            PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Infinite level");

            /*if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }*/
        }

        public void DeactivateInfiniteLevel()
        {
            Debug.Log("Infinite level deactivated!");
            LastScene = PlayerPrefs.GetInt("LastScene");
            SceneManager.LoadScene(LastScene);
        }

        public void GyroInputActivate()
        {
            gyrobuttonUI.SetActive(true);
            gyrodeactivatebuttonUI.SetActive(false);
            joystickUI.SetActive(false);
            gyroactive = 1;
        }

        public void GyroInputDeactivate()
        {
            gyrobuttonUI.SetActive(false);
            gyrodeactivatebuttonUI.SetActive(true);
            joystickUI.SetActive(true);
            gyroactive = 0;
        }

        public void WinAnimation()
        {
            //animator.CrossFade("Win", 1, -1);
        }

        public void FreezePlayerContraints()
        {
            FindObjectOfType<GameScript>().rb.constraints = RigidbodyConstraints.FreezeRotationY;
            //RenderSettings.fog = false;
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().name == "Cheat level")
            {
                RestartGame();
            }

            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void ReturnHome()
        {
            SceneManager.LoadScene("Home");
        }

        public void LevelChange()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: EASY";
                SceneManager.LoadScene("Easy level 1");
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                FindObjectOfType<ClearObstacles>().InfiniteLevel();
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: DEFAULT";
                SceneManager.LoadScene("Default level");
            }

            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: DEFAULT";
                SceneManager.LoadScene("Default level");
            }

            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: DEFAULT";
                SceneManager.LoadScene("Default level");
            }
        }
    }
}
