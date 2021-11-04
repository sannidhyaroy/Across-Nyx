using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace MainScript
{
    public class WelcomeScreenScript : MonoBehaviour
    {
        public int lastscene;
        public AudioMixer audioMixer;
        public Animator animator;
        public GameObject StartMenu;
        public GameObject OptionsMenu;

        private void Awake()
        {
            OptionsMenu.SetActive(false);
        }

        // Start is called before the first frame update
        public void StartGame()
        {
            lastscene = PlayerPrefs.GetInt("LastScene", 3);
            if (lastscene == 2)
            {
                lastscene = 1;
            }
            PlayerPrefs.SetInt("LastScene", lastscene);
            PlayerPrefs.SetInt("cheatbuttonstatus", 0);
            SceneManager.LoadScene(lastscene);
        }
        public void Options()
        {
            if (!OptionsMenu.activeInHierarchy)
            {
                //animator.SetBool("options", true);
                StartMenu.SetActive(true);
                animator.Play("Options Menu");
                Invoke("StartMenuDisable", 1);
            }
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("Master Volume", volume);
        }

        public void Back()
        {
            OptionsMenu.SetActive(true);
            StartMenu.SetActive(true);
            animator.Play("Options Menu Out");
            Invoke("OptionsMenuDisable", 1);
            StartMenu.SetActive(true);
            //animator.SetBool("options", false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void OptionsMenuDisable()
        {
            OptionsMenu.SetActive(false);
        }

        public void StartMenuDisable()
        {
            StartMenu.SetActive(false);
        }
    }
}
