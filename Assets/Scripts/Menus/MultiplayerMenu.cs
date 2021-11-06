using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class MultiplayerMenu : MonoBehaviour
    {
        public void OnClick_Back()
        {
            MenuManager.OpenMenu(Menu.Main_Menu, this.gameObject);
        }
    }
}