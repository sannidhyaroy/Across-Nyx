using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

namespace MainScript
{
    public class PlayFabManager : MonoBehaviour
    {
        [Header("Leaderboard Options")]
        public GameObject rowPrefab;
        public Transform rowsParent;
        [SerializeField] private GameObject leaderboardLoading;
        [Header("UI Options")]
        public GameObject StartMenu;
        public GameObject UsernameUI;
        public GameObject LoginUI;
        public GameObject ForgotPasswordUI;
        public GameObject LoadingUI;
        public TextMeshProUGUI ConsoleMsg;
        public TextMeshProUGUI ResetMsg;
        public TextMeshProUGUI UsernameMsg;
        public TextMeshProUGUI FeedbackMsg;
        public TextMeshProUGUI displayname;
        [Header("Others")]
        public Text PlayerName;
        public TextMeshProUGUI registeredEmail;
        public TextMeshProUGUI playertitleid;
        public TMP_InputField emailid;
        public TMP_InputField password;
        public TMP_InputField resetemailid;
        public TMP_InputField username;
        public TMP_InputField feedbackinput;
        [HideInInspector] public string playerid = null;

        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                return;
            }
            /*emailid.text = PlayerPrefs.GetString("Email ID", null); // storing sensitive information using PlayerPrefs is highly insecure
            password.text = PlayerPrefs.GetString("Password", null); // storing sensitive information using PlayerPrefs is highly insecure

            if (PlayerPrefs.GetString("Password", "0") != "0") // storing sensitive information using PlayerPrefs is highly insecure
            {
                ConsoleMsg.text = "Your login details are autofilled based on your previous logins. Change them if necessary!";
            }*/
            PlayerPrefs.DeleteKey("Email ID"); // delete previous login details saved by PlayerPrefs as they're highly insecure
            PlayerPrefs.DeleteKey("Password"); // delete previous login details saved by PlayerPrefs as they're highly insecure
            GameData data = SaveGameData.LoadData();
            if (data != null)
            {
                emailid.text = Encryption.EncryptDecrypt(data.EmailID, 500);
                password.text = Encryption.EncryptDecrypt(data.Password, 2000);
                ConsoleMsg.text = "Your login details are autofilled based on your previous logins. Change them if necessary!";
            }
        }

        public void RegisterButton()
        {
            if (password.text.Length < 6)
            {
                ConsoleMsg.text = "Password too short!";
                Invoke("MessageClear", 3);
                return;
            }
            var request = new RegisterPlayFabUserRequest
            {
                Email = emailid.text,
                Password = password.text,
                RequireBothUsernameAndEmail = false,
            };
            LoadingUI.SetActive(true);
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        }

        public void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            LoadingUI.SetActive(false);
            Debug.Log("Registered and logged in!");
            ConsoleMsg.text = "Registered and logged in!";
            Invoke("MessageClear", 3);
            registeredEmail.text = "Email ID: " + emailid.text;
            playertitleid.text = "Player ID: (Log back in to see your ID)";
            PlayerPrefs.DeleteKey("displayname");
            PlayerPrefs.DeleteKey("playertitleid");

            //PlayerPrefs.SetString("Email ID", emailid.text); // storing sensitive information using PlayerPrefs is highly insecure
            //PlayerPrefs.SetString("Password", password.text); // storing sensitive information using PlayerPrefs is highly insecure
            SaveGameData.SaveData(this);

            /*LoginUI.SetActive(false);
            StartMenu.SetActive(true);*/
            LoginUI.SetActive(false);
            UsernameUI.SetActive(true);
        }

        public void OnError(PlayFabError error)
        {
            LoadingUI.SetActive(false);
            Debug.Log("Couldn't register!");
            ConsoleMsg.text = error.ErrorMessage;
            ResetMsg.text = error.ErrorMessage;
            UsernameMsg.text = error.ErrorMessage;
            FeedbackMsg.text = error.ErrorMessage;
            if (ConsoleMsg.text == "User not found")
            {
                emailid.text = null;
            }
            Invoke("MessageClear", 3);
            password.text = null;
            resetemailid.text = null;
        }

        public void LoginButton()
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = emailid.text,
                Password = password.text,
                //TitleId = "EF1D9",
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            LoadingUI.SetActive(true);
            PlayerPrefs.DeleteKey("displayname");
            PlayerPrefs.DeleteKey("playertitleid");
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        }

        public void OnLoginSuccess(LoginResult result)
        {
            LoadingUI.SetActive(false);
            Debug.Log("Logged in!");
            ConsoleMsg.text = "BOOM! You're in!!";
            Invoke("MessageClear", 3);
            LoginUI.SetActive(false);
            registeredEmail.text = "Email ID: " + emailid.text;

            //PlayerPrefs.SetString("Email ID", emailid.text); // storing sensitive information using PlayerPrefs is highly insecure
            //PlayerPrefs.SetString("Password", password.text); // storing sensitive information using PlayerPrefs is highly insecure
            SaveGameData.SaveData(this);

            if (result.InfoResultPayload.PlayerProfile != null)
            {
                playerid = result.InfoResultPayload.PlayerProfile.DisplayName;
                PlayerPrefs.SetString("displayname", playerid); // storing sensitive information using PlayerPrefs is highly insecure
            }
            playertitleid.text = "Player ID: " + result.InfoResultPayload.PlayerProfile.PlayerId;
            PlayerPrefs.SetString("playertitleid", result.InfoResultPayload.PlayerProfile.PlayerId);
            if (playerid == null)
            {
                UsernameUI.SetActive(true);
            }

            else
            {
                displayname.text = "Username: " + playerid;
                PlayerName.text = playerid;
                StartMenu.SetActive(true);
            }
            //FindObjectOfType<CollectablesManager>().RestoreCoins();
            //FindObjectOfType<CollectablesManager>().RestoreGems();
            GetProgress();
        }
        public void AnonymousLogin()
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = "nyxgamer@notoncomics.tk",
                Password = "nyxgamer"
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, AnonymousLoginSuccess, AnonymousLoginError);
        }
        public void AnonymousLoginSuccess(LoginResult result)
        {
            Debug.Log("Logged in anonymously :)");
            GetTitleData();
        }
        public void AnonymousLoginError(PlayFabError error)
        {
            Debug.Log("Anonymously login failed!");
            Debug.Log("No internet!");
            FindObjectOfType<UpdateCheck>().NoInternetAlert.SetActive(true);
            FindObjectOfType<UpdateCheck>().CheckingForUpdatesUI.SetActive(false);
            FindObjectOfType<UpdateCheck>().UpdateScreen.SetActive(false);
            FindObjectOfType<UpdateCheck>().WelcomeScreen.SetActive(false);
            FindObjectOfType<UpdateCheck>().LoginUI.SetActive(false);
            FindObjectOfType<UpdateCheck>().Character.SetActive(false);
        }
        public void AnonymousLogout()
        {
            PlayFabClientAPI.ForgetAllCredentials();
            Debug.Log("Logged out anonymously :)");
        }
        public void ResetPassword()
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = resetemailid.text,
                TitleId = "EF1D9"
            };
            LoadingUI.SetActive(true);
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
        }

        public void OnPasswordReset(SendAccountRecoveryEmailResult result)
        {
            LoadingUI.SetActive(false);
            Debug.Log("Account Recovery Link is sent successfully to you registered email ID");
            ResetMsg.text = "Account Recovery Link is sent successfully to you registered email ID";
            Invoke("MessageClear", 3);
        }

        public void ForgotPassword()
        {
            LoginUI.SetActive(false);
            ForgotPasswordUI.SetActive(true);
        }

        public void CancelButton()
        {
            ForgotPasswordUI.SetActive(false);
            LoginUI.SetActive(true);
        }

        public void UsernameProceedButton()
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = username.text
            };
            LoadingUI.SetActive(true);
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
            displayname.text = "Display Name: " + username.text;
            PlayerName.text = username.text;
            PlayerPrefs.SetString("displayname", username.text);
        }

        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            LoadingUI.SetActive(false);
            Debug.Log("Display Name updated!");
            UsernameUI.SetActive(false);
            StartMenu.SetActive(true);
        }

        public void LogOutButton()
        {
            PlayFabClientAPI.ForgetAllCredentials();
            ScriptAttemptManager.ScriptAttempts = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MessageClear()
        {
            ConsoleMsg.text = null;
            ResetMsg.text = null;
        }

        public void SendTotalScore(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "Nyx Adventure total score points",
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdateSuccess, OnError);
        }

        public void GetTotalScore()
        {
            // foreach (Transform item in rowsParent)
            // {
            //     Destroy(item.gameObject);
            // }
            // leaderboardLoading.SetActive(true);
            var request = new GetLeaderboardRequest
            {
                StatisticName = "Nyx Adventure total score points",
                StartPosition = 0,
                MaxResultsCount = 10
            };
            PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
        }

        public void OnLeaderboardUpdateSuccess(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Score sent to PlayFab Leaderboard");
        }

        public void OnLeaderBoardGet(GetLeaderboardResult result)
        {
            foreach (Transform item in rowsParent)
            {
                Destroy(item.gameObject);
            }
            Debug.Log("PlayFab Leaderboard retrieved!");
            // leaderboardLoading.SetActive(false);
            foreach (var item in result.Leaderboard)
            {
                GameObject ScoreTile = Instantiate(rowPrefab, rowsParent);
                TextMeshProUGUI[] texts = ScoreTile.GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = (item.Position + 1).ToString();
                texts[1].text = item.PlayFabId;
                texts[2].text = item.DisplayName;
                texts[3].text = item.StatValue.ToString();
                if ((item.Position) <= 3)
                {
                    switch (item.Position + 1)
                    {
                        case 1:
                            foreach (var text in texts)
                            {
                                text.color = new Color32(255, 215, 0, 255);
                            }
                            break;
                        case 2:
                            foreach (var text in texts)
                            {
                                text.color = new Color32(147, 174, 188, 255);
                            }
                            break;
                        case 3:
                            foreach (var text in texts)
                            {
                                text.color = new Color32(205, 127, 50, 255);
                            }
                            break;

                    }
                }
                if (item.PlayFabId == PlayerPrefs.GetString("playertitleid", null))
                {
                    foreach (var text in texts)
                    {
                        text.color = new Color32(255, 138, 101, 255);
                    }
                }
                // Debug.Log("Player Position: " + item.Position + 1 + ", Player ID: " + item.PlayFabId + ", Player Name: " + item.DisplayName + ", Player Score: " + item.StatValue);
            }
        }
        public void SubmitFeedback()
        {
            if (feedbackinput.text == null)
            {
                FeedbackMsg.text = "Feedback field is empty!";
                return;
            }
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "sendFeedback",
                FunctionParameter = new
                {
                    displayname = playerid,
                    message = feedbackinput.text
                }
            };
            LoadingUI.SetActive(true);
            PlayFabClientAPI.ExecuteCloudScript(request, OnFeedbackSubmitSuccess, OnError);
        }
        public void OnFeedbackSubmitSuccess(ExecuteCloudScriptResult result)
        {
            LoadingUI.SetActive(false);
            FeedbackMsg.text = "Feedback successfully sent!";
            feedbackinput.text = null;
        }
        public void SaveProgress()
        {
            FindObjectOfType<CollectablesManager>().LoadCollectables();
            int Coins = FindObjectOfType<CollectablesManager>().TotalCoinsAvailable;
            int Gems = FindObjectOfType<CollectablesManager>().TotalGemsAvailable;
            Debug.Log("Total Coins Available integer set to " + Coins);
            Debug.Log("Total Gems Available integer set to " + Gems);
            string Coin = (FindObjectOfType<CoinsController>().CoinCollected + Coins).ToString();
            string Gem = (FindObjectOfType<GemsController>().GemCollected + Gems).ToString();
            Debug.Log("Sending " + Coin + "as coin data");
            Debug.Log("Sending " + Gem + "as gem data");
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"CoinsAvailable", Coin},
                    {"GemsAvailable", Gem}
                }
            };
            FindObjectOfType<CollectablesManager>().UpdateCollectables(int.Parse(Coin), int.Parse(Gem));
            PlayFabClientAPI.UpdateUserData(request, OnDataSent, OnError);
        }
        public void GetProgress()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnDataRequestError);
        }
        public void OnDataSent(UpdateUserDataResult result)
        {
            Debug.Log("Data updated successfully on PlayFab Servers!");
        }
        public void OnDataReceived(GetUserDataResult result)
        {
            Debug.Log("Data received successfully from PlayFab Servers!");
            if (result.Data != null && result.Data.ContainsKey("CoinsAvailable") && result.Data.ContainsKey("GemsAvailable"))
            {
                FindObjectOfType<CollectablesManager>().RestoreCoins(int.Parse(result.Data["CoinsAvailable"].Value));
                FindObjectOfType<CollectablesManager>().RestoreGems(int.Parse(result.Data["GemsAvailable"].Value));
                FindObjectOfType<CollectablesManager>().TotalCoinsAvailable = int.Parse(result.Data["CoinsAvailable"].Value);
                FindObjectOfType<CollectablesManager>().TotalGemsAvailable = int.Parse(result.Data["GemsAvailable"].Value);
            }
            else
            {
                FindObjectOfType<CollectablesManager>().RestoreCoins(0);
                FindObjectOfType<CollectablesManager>().RestoreGems(0);
                FindObjectOfType<CollectablesManager>().TotalCoinsAvailable = 0;
                FindObjectOfType<CollectablesManager>().TotalGemsAvailable = 0;
                Debug.Log("Player Collectables Data not found on PlayFab Servers!");
            }
            FindObjectOfType<CollectablesManager>().StoreCollectables();
            Debug.Log("Total Coins Available integer set to " + FindObjectOfType<CollectablesManager>().TotalCoinsAvailable);
            Debug.Log("Total Gems Available integer set to " + FindObjectOfType<CollectablesManager>().TotalGemsAvailable);
        }
        public void OnDataRequestError(PlayFabError error)
        {
            FindObjectOfType<CollectablesManager>().CoinsAvailable.text = "error";
            FindObjectOfType<CollectablesManager>().GemsAvailable.text = "error";
        }
        public void GetTitleData()
        {
            //AnonymousLogin();
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceived, OnError);
        }
        public void OnTitleDataReceived(GetTitleDataResult result)
        {
            AnonymousLogout();
            if (result.Data == null || result.Data.ContainsKey("Latest Version") == false)
            {
                FindObjectOfType<UpdateCheck>().DataReceiveError();
                Debug.LogWarning("NO TITLE DATA RECEIVED!");
            }
            else
            {
                FindObjectOfType<UpdateCheck>().LatesttVer = result.Data["Latest Version"];
                FindObjectOfType<UpdateCheck>().Description.text = result.Data["Update Description"];
                FindObjectOfType<UpdateCheck>().GameLink = result.Data["Download link"];
                FindObjectOfType<UpdateCheck>().DataReceived();
            }
        }
    }
}
