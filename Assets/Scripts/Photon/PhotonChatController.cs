using System;
using Photon.Pun;
using Photon.Chat;
using UnityEngine;
using ExitGames.Client.Photon;

namespace MainScript
{
    public class PhotonChatController : MonoBehaviour, IChatClientListener
    {
        [SerializeField] private string UserID;
        private ChatClient chatClient;

        #region Unity Methods
        public void StartPhotonChatService()
        {
            UserID = PlayerProfile.Username;
            chatClient = new ChatClient(this);
            ConnectToPhotonChat();
        }

        private void Update()
        {
            chatClient.Service();
        }
        #endregion

        #region Private Methods
        private void ConnectToPhotonChat()
        {
            Debug.Log("Connecting to Photon Chat...");
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(UserID);
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(UserID));
        }
        #endregion

        #region Public Methods
        public void SendDirectMessage(string recipient, string message)
        {
            chatClient.SendPrivateMessage(recipient, message);
        }
        #endregion

        #region Photon Chat Callbacks
        public void DebugReturn(DebugLevel level, string message)
        {
            Debug.Log($"Photon Chat DebugReturn: {message}");
        }

        public void OnDisconnected()
        {
            Debug.Log("Disconnected from the Photon Chat Server");
        }

        public void OnConnected()
        {
            Debug.Log("Connected to the Photon Chat Server");
            SendDirectMessage("Sanhita", "Hi");
        }

        public void OnChatStateChange(ChatState state)
        {
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            if (!string.IsNullOrEmpty(message.ToString()))
            {
                // Channel Name format [Sender : Recipient]
                string[] splitNames = channelName.Split(new char[] { ':' });
                string senderName = splitNames[0];
                if (sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log(sender + " : " + message);
                }
            }
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
        }

        public void OnUnsubscribed(string[] channels)
        {
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

        public void OnUserSubscribed(string channel, string user)
        {
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
        }
        #endregion
    }
}