using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class FeedbackMenu : MonoBehaviour
    {
        public void OnClick_Cancel()
        {
            MenuManager.OpenMenu(Menu.Main_Menu, this.gameObject);
        }
    }
}