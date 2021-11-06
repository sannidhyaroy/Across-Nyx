using Photon.Pun;
using UnityEngine;

namespace MainScript
{
    public class MultiplayerMenu : MonoBehaviour
    {
        [SerializeField] PhotonChatController chatController;
        public void OnClick_Back()
        {
            MenuManager.OpenMenu(Menu.Main_Menu, this.gameObject);
            chatController.chatClient.Disconnect();
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }
            }
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.Disconnect();
        }
    }
}