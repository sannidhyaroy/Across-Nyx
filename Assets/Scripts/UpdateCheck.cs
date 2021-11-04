using UnityEngine;
using TMPro;
namespace MainScript
{
    public class UpdateCheck : MonoBehaviour
    {
        [HideInInspector] public string CurrentVer;
        [HideInInspector] public string LatesttVer;
        [HideInInspector] public string GameLink;
        [Header("UI Objects")]
        public GameObject UpdateScreen;
        public GameObject WelcomeScreen;
        public GameObject LoginUI;
        public GameObject Character;
        public GameObject NoInternetAlert;
        public GameObject NoUpdatesUI;
        public GameObject CheckingForUpdatesUI;
        [Header("Animator")]
        [SerializeField] private Animator animator;
        [Header("Check for Updates settings")]
        public TextMeshProUGUI Description;
        [SerializeField] TextMeshProUGUI LatestVersionText;
        [HideInInspector] public int tryagainattempts = 0;

        public void Awake()
        {
            if (!ScriptAttemptManager.ScriptAttempts)
            {
                Time.timeScale = 1;
                ScriptAttemptManager.ScriptAttempts = true;
                // WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                CheckingForUpdatesUI.SetActive(true);
                CurrentVer = Application.version;
                FindObjectOfType<PlayFabManager>().AnonymousLogin();
                return;
            }
            WelcomeScreen.SetActive(true);
            FindObjectOfType<PlayFabManager>().GetProgress();
            FindObjectOfType<PlayFabManager>().playertitleid.text = "Player ID: " + PlayerPrefs.GetString("playertitleid", "(Log back in to see your ID)");
            FindObjectOfType<PlayFabManager>().displayname.text = "Username: " + PlayerPrefs.GetString("displayname");
            FindObjectOfType<PlayFabManager>().PlayerName.text = PlayerPrefs.GetString("displayname");
            GameData data = SaveGameData.LoadData();
            FindObjectOfType<PlayFabManager>().registeredEmail.text = "Email ID: " + Encryption.EncryptDecrypt(data.EmailID, 500);
        }
        public void CheckVersion()
        {
            float CurrentVersion, LatestVersion;
            if (float.TryParse(CurrentVer, out CurrentVersion) && float.TryParse(LatesttVer, out LatestVersion))
            {
                if (float.Parse(CurrentVer) < float.Parse(LatesttVer))
                {
                    UpdateScreen.SetActive(true);
                    WelcomeScreen.SetActive(false);
                    LoginUI.SetActive(false);
                    Character.SetActive(false);
                }
                else
                {
                    Continue();
                }
            }
            else
            {
                Debug.LogAssertion("You seem to be running a development build of Across Nyx Project!");
                Continue();
            }
            LatestVersionText.text = "Latest Version: " + LatesttVer;
            Debug.Log("Current Version: " + CurrentVer + "  Latest Version: " + LatesttVer);
        }
        public void UpdateNow()
        {
            Application.OpenURL(GameLink);
        }
        public void NotNow()
        {
            UpdateScreen.SetActive(false);
            //WelcomeScreen.SetActive(true);
            LoginUI.SetActive(true);
            Character.SetActive(true);
        }
        public void TryAgain()
        {
            tryagainattempts++;
            NoInternetAlert.SetActive(false);
            CheckingForUpdatesUI.SetActive(true);
            //Start();
            FindObjectOfType<PlayFabManager>().AnonymousLogin();
        }
        public void Skip()
        {
            Application.Quit();
            NoInternetAlert.SetActive(false);
            //WelcomeScreen.SetActive(true);
            LoginUI.SetActive(true);
            Character.SetActive(true);
        }
        public void NoUpdatesOkayButton()
        {
            NoUpdatesUI.SetActive(false);
            //WelcomeScreen.SetActive(true);
            LoginUI.SetActive(true);
            Character.SetActive(true);
        }
        public void DataReceived()
        {
            animator.SetTrigger("end");
            Invoke("CheckVersion", 1f);
        }
        public void DataReceiveError()
        {
            Debug.Log("No internet!");
            NoInternetAlert.SetActive(true);
            UpdateScreen.SetActive(false);
            WelcomeScreen.SetActive(false);
            LoginUI.SetActive(false);
            Character.SetActive(false);
        }
        private void Continue()
        {
            if (tryagainattempts == 0)
            {
                UpdateScreen.SetActive(false);
                //WelcomeScreen.SetActive(true);
                LoginUI.SetActive(true);
                Character.SetActive(true);
            }
            else
            {
                NoUpdatesUI.SetActive(true);
                WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                Character.SetActive(false);
            }
        }
    }
}
