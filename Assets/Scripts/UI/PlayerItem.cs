using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public void SetPlayerInfo(Player player)
    {
        playerName.text = player.UserId;
    }
}
