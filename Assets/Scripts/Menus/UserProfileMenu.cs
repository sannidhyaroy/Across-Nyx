using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfileMenu : MonoBehaviour
{
    public void OnClick_Close()
    {
        MenuManager.OpenMenu(Menu.Main_Menu, this.gameObject);
    }
}
