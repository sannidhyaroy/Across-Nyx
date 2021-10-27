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
        public GameObject OptionsButton;
        public GameObject UserProfileUI;
        public GameObject UserProfileButton;
        public GameObject UserProfileExitButton;
        public GameObject CoinStackUI;
        public GameObject GemStackUI;
        public GameObject FeedbackFormButton;
        public GameObject FeedbackFormUI;

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

        public void OnUserProfileButtonClick()
        {
            UserProfileExitButton.SetActive(true);
            UserProfileButton.SetActive(false);
            StartMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            OptionsButton.SetActive(false);
            CoinStackUI.SetActive(false);
            GemStackUI.SetActive(false);
            FeedbackFormButton.SetActive(false);
            UserProfileUI.SetActive(true);
            FindObjectOfType<UpdateCheck>().Character.SetActive(false);
        }

        public void OnUserProfileExitButtonClick()
        {
            //animator.Play("Profile Panel Exit Anim");
            UserProfileExitButton.SetActive(false);
            UserProfileButton.SetActive(true);
            UserProfileUI.SetActive(false);
            StartMenu.SetActive(true);
            OptionsButton.SetActive(true);
            CoinStackUI.SetActive(true);
            GemStackUI.SetActive(true);
            FeedbackFormButton.SetActive(true);
            FindObjectOfType<UpdateCheck>().Character.SetActive(true);
        }
        public void ExitProfileEvent()
        {
            UserProfileExitButton.SetActive(false);
            UserProfileButton.SetActive(true);
            UserProfileUI.SetActive(false);
            StartMenu.SetActive(true);
            OptionsButton.SetActive(true);
            CoinStackUI.SetActive(true);
            GemStackUI.SetActive(true);
            FeedbackFormButton.SetActive(true);
            FindObjectOfType<UpdateCheck>().Character.SetActive(true);
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
        public void FeedBackFormButton()
        {
            StartMenu.SetActive(false);
            OptionsButton.SetActive(false);
            OptionsMenu.SetActive(false);
            CoinStackUI.SetActive(false);
            GemStackUI.SetActive(false);
            UserProfileButton.SetActive(false);
            UserProfileExitButton.SetActive(false);
            FeedbackFormButton.SetActive(false);
            FeedbackFormUI.SetActive(true);
        }
        public void FeedBackFormCancelButton()
        {
            FeedbackFormUI.SetActive(false);
            StartMenu.SetActive(true);
            OptionsButton.SetActive(true);
            CoinStackUI.SetActive(true);
            GemStackUI.SetActive(true);
            FeedbackFormButton.SetActive(true);
            UserProfileButton.SetActive(true);
        }
    }
}
