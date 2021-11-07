using UnityEngine;

namespace MainScript
{
    public static class MenuManager
    {
        public static GameObject MainMenu, OptionsMenu, StoreMenu, LeaderboardMenu, MultiplayerMenu, PhotonLobby, PhotonRoom, UserProfileMenu, FeedbackMenu;
        public static void Init()
        {
            GameObject canvas = GameObject.Find("Home Canvas");
            MainMenu = canvas.transform.Find("Home Panel").gameObject;
            OptionsMenu = canvas.transform.Find("Options Panel").gameObject;
            StoreMenu = canvas.transform.Find("Store Panel").gameObject;
            LeaderboardMenu = canvas.transform.Find("Leaderboard Panel").gameObject;
            MultiplayerMenu = canvas.transform.Find("Multiplayer Panel").gameObject;
            PhotonLobby = MultiplayerMenu.transform.Find("Lobby Panel").gameObject;
            PhotonRoom = MultiplayerMenu.transform.Find("Room Panel").gameObject;
            UserProfileMenu = canvas.transform.Find("User Profile Panel").gameObject;
            FeedbackMenu = canvas.transform.Find("Feedback Form Panel").gameObject;
        }
        public static void OpenMenu(Menu menu, GameObject callingMenu, bool activateCharacter = true)
        {
            if (!MainMenu)
            {
                Init();
            }
            switch (menu)
            {
                case Menu.Main_Menu:
                    MainMenu.SetActive(true);
                    break;

                case Menu.Options:
                    OptionsMenu.SetActive(true);
                    break;

                case Menu.Store:
                    StoreMenu.SetActive(true);
                    break;

                case Menu.Leaderboard:
                    LeaderboardMenu.SetActive(true);
                    break;
                    
                case Menu.Multiplayer:
                    MultiplayerMenu.SetActive(true);
                    break;
                    
                case Menu.PhotonLobby:
                    PhotonLobby.SetActive(true);
                    break;
                    
                case Menu.PhotonRoom:
                    PhotonRoom.SetActive(true);
                    break;
                    
                case Menu.User_Profile:
                    UserProfileMenu.SetActive(true);
                    break;
                    
                case Menu.FeedbackForm:
                    FeedbackMenu.SetActive(true);
                    break;
            }
            callingMenu.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<UpdateCheck>().Character.SetActive(activateCharacter);
        }
    }
}
