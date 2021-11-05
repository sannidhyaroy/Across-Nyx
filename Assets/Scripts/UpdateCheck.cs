//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using TMPro;

namespace MainScript
{
    public class UpdateCheck : MonoBehaviour
    {
        // Start is called before the first frame update
        [HideInInspector] public string CurrentVer;

        //[HideInInspector] public string URL = "https://drive.google.com/uc?export=download&id=1LKur2q5JBnFMCg8FKktbAzGx-DWJsPhc";

        //[HideInInspector] public string updatelinktext = "https://drive.google.com/uc?export=download&id=1zK8mDFiVfNm-M7b8eFDSntL-4HSVb-J0";

        //[HideInInspector] public string descriptionlinktext = "https://drive.google.com/uc?export=download&id=1VW4uE9XuBcqCKFNM3d7s7QMSEC-Bj53j";

        //[HideInInspector] public string clearplayerprefscmdlink = "https://pastebin.com/raw/8mfKKTHK";

        [HideInInspector] public string LatesttVer;

        [HideInInspector] public string GameLink;

        //[HideInInspector] public string clearplayerprefs;

        public GameObject UpdateScreen;

        public GameObject WelcomeScreen;
        public GameObject LoginUI;

        public GameObject Character;

        public GameObject NoInternetAlert;

        public GameObject NoUpdatesUI;

        public GameObject CheckingForUpdatesUI;

        [SerializeField] private Animator animator;

        public TextMeshProUGUI Description;

        [SerializeField] TextMeshProUGUI LatestVersionText;

        [HideInInspector] public int tryagainattempts = 0;

        public void Awake()
        {
            if (!ScriptAttemptManager.ScriptAttempts)
            {
                Time.timeScale = 1;
                ScriptAttemptManager.ScriptAttempts = true;
                WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                CheckingForUpdatesUI.SetActive(true);
                CurrentVer = Application.version;
                FindObjectOfType<PlayFabManager>().AnonymousLogin();
                return;
            }
            WelcomeScreen.SetActive(true);
            FindObjectOfType<PlayFabManager>().GetProgress();
            FindObjectOfType<PlayFabManager>().UserProfileDataInit();
        }

        /*public void Start()
        {
            StartCoroutine(LoadTextData(URL));
            StartCoroutine(LoadGameUrlData(updatelinktext));
            StartCoroutine(LoadUpdateDescription(descriptionlinktext));
            //StartCoroutine(ClearPlayerPrefs(clearplayerprefscmdlink));
        }*/

        public void CheckVersion()
        {
            float CurrentVersion;
            float LatestVersion;
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


        /*IEnumerator LoadTextData(string url)
        {
            //WWW www = new WWW(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            LatesttVer = www.downloadHandler.text;
            Debug.Log("Latest Version available: " + LatesttVer);

            if (LatesttVer == "")
            {
                Debug.Log("No internet!");
                NoInternetAlert.SetActive(true);
                CheckingForUpdatesUI.SetActive(false);
                UpdateScreen.SetActive(false);
                WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                Character.SetActive(false);
            }

            else
            {
                //CheckingForUpdatesUI.SetActive(false);
                animator.SetTrigger("end");
                Invoke("CheckVersion", 1f);
            }
        }

        IEnumerator LoadGameUrlData(string url)
        {
            //WWW www = new WWW(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            GameLink = www.downloadHandler.text;
            Debug.Log("Game link: " + GameLink);
        }

        IEnumerator LoadUpdateDescription(string url)
        {
            //WWW www = new WWW(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            Description.text = www.downloadHandler.text;
            Debug.Log("Description: " + Description.text);
            if (Description.text == "")
            {
                Debug.Log("No internet!");
                NoInternetAlert.SetActive(true);
                UpdateScreen.SetActive(false);
                WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                Character.SetActive(false);
            }
        }*/

        /*IEnumerator ClearPlayerPrefs(string url)
        {
            //WWW www = new WWW(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            clearplayerprefs = www.downloadHandler.text;
            Debug.Log("Clear PlayerPrefs command is set to " + www.downloadHandler.text);
            if (clearplayerprefs == "true")
            {
                Debug.Log("PlayerPrefs reset command initiated!");
                PlayerPrefs.DeleteAll();
            }
        }*/

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
        /*IEnumerator checkInternetConnection()
        {
            UnityWebRequest www = UnityWebRequest.Get(URL);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.downloadHandler.text == null)
            {
                Debug.Log("No internet!");
                NoInternetAlert.SetActive(true);
                CheckingForUpdatesUI.SetActive(false);
                UpdateScreen.SetActive(false);
                WelcomeScreen.SetActive(false);
                LoginUI.SetActive(false);
                Character.SetActive(false);
            }
            else
            {
                FindObjectOfType<PlayFabManager>().AnonymousLogin();
            }
        }*/
    }
}
