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
        public ChatClient chatClient;
        public static Action<string, string> OnRoomInvite = delegate { };
        public static Action<ChatClient> OnChatConnected = delegate { };
        public static Action<PhotonStatus> OnStatusUpdated = delegate { };


        #region Unity Methods
        private void Awake() {
            FriendsUIList.OnInviteFriend += HandleFriendInvite;
        }
        private void OnDestroy() {
            FriendsUIList.OnInviteFriend -= HandleFriendInvite;
        }

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
        public void HandleFriendInvite(string recipient)
        {
            if (string.IsNullOrEmpty(PhotonNetwork.CurrentRoom.Name))
            {
                Debug.LogWarning("You need to create or join a room to invite a friend!");
                return;
            }
            chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
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
            chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        }

        public void OnConnected()
        {
            Debug.Log("Connected to the Photon Chat Server");
            OnChatConnected?.Invoke(chatClient);
            chatClient.SetOnlineStatus(ChatUserStatus.Online);
            // SendDirectMessage("Sanhita", "Hi");
        }

        public void OnChatStateChange(ChatState state)
        {
            Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            Debug.Log($"Photon Chat OnGetMessages {channelName}");
            for (int i = 0; i < senders.Length; i++)
            {
                Debug.Log($"{senders[i]} messaged: {messages[i]}");
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            if (!string.IsNullOrEmpty(message.ToString()))
            {
                // Channel Name format [Sender : Recipient]
                string[] splitNames = channelName.Split(new char[] { ':' });
                string senderName = splitNames[0];
                if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log(sender + " : " + message);
                    OnRoomInvite?.Invoke(sender, message.ToString());
                }
            }
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            Debug.Log($"Photon Chat OnSubscribed");
            for (int i = 0; i < channels.Length; i++)
            {
                Debug.Log($"{channels[i]}");
            }
        }

        public void OnUnsubscribed(string[] channels)
        {
            Debug.Log($"Photon Chat OnUnsubscribed");
            for (int i = 0; i < channels.Length; i++)
            {
                Debug.Log($"{channels[i]}");
            }
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
            PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
            Debug.Log($"Status Update for {user} and its now {status}.");
            OnStatusUpdated?.Invoke(newStatus);
        }

        public void OnUserSubscribed(string channel, string user)
        {
            Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
        }
        #endregion
    }
}