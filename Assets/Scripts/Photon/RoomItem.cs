using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainScript
{
    public class RoomItem : MonoBehaviour
    {
        private PhotonConnector connector;
        public TextMeshProUGUI roomItemName;
        private void Start() {
            connector = FindObjectOfType<PhotonConnector>();
        }
        public void SetRoomName(string roomName)
        {
            roomItemName.text = roomName;

        }
        public void OnClick_Item()
        {
            connector.OnClick_JoinRoom(roomItemName.text);
        }
    }
}