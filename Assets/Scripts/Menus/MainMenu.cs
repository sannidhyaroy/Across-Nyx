using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class MainMenu : MonoBehaviour
    {
        public void OnClick_MainMenu()
        {
            MenuManager.OpenMenu(Menu.Main_Menu, this.gameObject);
        }
        public void OnClick_Options()
        {
            MenuManager.OpenMenu(Menu.Options, this.gameObject, false);
        }
        public void OnClick_Store()
        {
            MenuManager.OpenMenu(Menu.Store, this.gameObject, false);
        }
        public void OnClick_Leaderboard()
        {
            MenuManager.OpenMenu(Menu.Leaderboard, this.gameObject, false);
        }
        public void OnClick_Profile()
        {
            MenuManager.OpenMenu(Menu.User_Profile, this.gameObject, false);
        }
        public void OnClick_Feedback()
        {
            MenuManager.OpenMenu(Menu.FeedbackForm, this.gameObject);
        }
    }
}
