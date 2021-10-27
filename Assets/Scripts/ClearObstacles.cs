using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MainScript
{
    public class ClearObstacles : MonoBehaviour
    {

        //public GameObject obstacles;
        //public GameObject cheatbuttonUI;
        public GameObject infiniteactivatebutton;
        public GameObject infinitedeactivatebutton;
        private int infinitebuttonclick;
        //private int cheatbuttonclick;

        public void Start()
        {
            /*cheatbuttonclick = PlayerPrefs.GetInt("cheatbuttonstatus", 0);

            if (cheatbuttonclick == 1)
            {
                cheatbuttonUI.SetActive(true);
            }

            else
            {
                cheatbuttonUI.SetActive(false);
            }*/
            if (SceneManager.GetActiveScene().name == "Infinite level")
            {
                infinitebuttonclick = 1;
                infiniteactivatebutton.SetActive(true);
                infinitedeactivatebutton.SetActive(false);
            }
            else
            {
                infinitebuttonclick = 0;
                infiniteactivatebutton.SetActive(false);
                infinitedeactivatebutton.SetActive(true);
            }
        }

        /*public void RemoveObstacles()
        {
            if (cheatbuttonclick == 0)
            {
                //obstacles.SetActive(false);
                cheatbuttonclick = 1;
                cheatbuttonUI.SetActive(true);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: CHEAT";
                FindObjectOfType<GameManager>().ActivateCheatScene();
            }
            else
            {
                //obstacles.SetActive(true);
                cheatbuttonclick = 0;
                cheatbuttonUI.SetActive(false);
                FindObjectOfType<GameManager>().DeactivateCheatScene();
            }
            PlayerPrefs.SetInt("cheatbuttonstatus", cheatbuttonclick);
        }*/
        public void InfiniteLevel()
        {
            if (infinitebuttonclick == 0)
            {
                //obstacles.SetActive(false);
                infinitebuttonclick = 1;
                infiniteactivatebutton.SetActive(true);
                infinitedeactivatebutton.SetActive(false);
                FindObjectOfType<PauseMenu>().levelConfigure.text = "LEVEL: INFINITE";
                FindObjectOfType<GameManager>().ActivateInfiniteLevel();
            }
            else
            {
                //obstacles.SetActive(true);
                infinitebuttonclick = 0;
                infiniteactivatebutton.SetActive(false);
                infinitedeactivatebutton.SetActive(true);
                FindObjectOfType<GameManager>().DeactivateInfiniteLevel();
            }
        }
    }
}
