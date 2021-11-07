using System;
using Photon.Realtime;
using Photon.Chat;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainScript
{
    public class FriendsUIList : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI friendNameText;
        [SerializeField] private string friendName;
        [SerializeField] private bool isOnline;
        [SerializeField] private Image onlineImage;
        [SerializeField] private GameObject inviteButton;
        [SerializeField] private Color onlineColor;
        [SerializeField] private Color offlineColor;
        public static Action<string> OnRemoveFriend = delegate { };
        public static Action<string> OnInviteFriend = delegate { };
        public static Action<string> OnGetCurrentStatus = delegate { };
        public static Action OnGetRoomStatus = delegate { };

        private void Awake()
        {
            PhotonChatController.OnStatusUpdated += HandleStatusUpdated;
            PhotonChatFriendController.OnStatusUpdated += HandleStatusUpdated;
            PhotonRoomController.OnRoomStatusChange += HandleInRoom;
        }
        private void OnDestroy()
        {
            PhotonChatController.OnStatusUpdated -= HandleStatusUpdated;
            PhotonChatFriendController.OnStatusUpdated -= HandleStatusUpdated;
            PhotonRoomController.OnRoomStatusChange -= HandleInRoom;
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(friendName)) return;
            OnGetCurrentStatus?.Invoke(friendName);
            OnGetRoomStatus?.Invoke();
        }

        public void Initialize(FriendInfo friend)
        {
            // if (friend.IsOnline)
            // {
            //     if (friend.IsInRoom)
            //     {
            //         Debug.Log($"{friend.UserId} is online and currently in a room named '{friend.Room}'");
            //     }
            //     else
            //     {
            //         Debug.Log($"{friend.UserId} is online and not in room");
            //     }
            // }
            // else
            // {
            //     Debug.Log($"{friend.UserId} is currently offline");
            // }
            
            SetupUI();
        }
        public void Initialize(string friendName)
        {
            Debug.Log($"{friendName} is added");
            this.friendName = friendName;

            SetupUI();
            OnGetCurrentStatus?.Invoke(friendName);
            OnGetRoomStatus?.Invoke();
        }
        private void HandleStatusUpdated(PhotonStatus status)
        {
            if (string.Compare(friendName, status.PlayerName) == 0)
            {
                Debug.Log($"Updating status in UI for {status.PlayerName} to status {status.Status}");
                SetStatus(status.Status);
            }
        }
        private void SetStatus(int status)
        {
            Debug.Log("Chat User Status: " + status);
            if (status == ChatUserStatus.Online)
            {
                onlineImage.color = onlineColor;
                isOnline = true;
                OnGetRoomStatus?.Invoke();
            }
            else
            {
                onlineImage.color = offlineColor;
                isOnline = false;
                inviteButton.SetActive(false);
            }
            Debug.Log($"Online Image Color: {onlineImage.color.ToString()}");
        }

        private void HandleInRoom(bool inRoom)
        {
            Debug.Log($"Updating invite ui to {inRoom}");
            inviteButton.SetActive(inRoom);
        }

        private void SetupUI()
        {
            // Debug.Log("Friend Name shall be set to " + friendName);
            friendNameText.text = friendName;
            inviteButton.SetActive(false);
        }
        public void RemoveFriend()
        {
            OnRemoveFriend?.Invoke(friendName);
        }
        public void InviteFriend()
        {
            Debug.Log(friendName + "invited!");
            OnInviteFriend?.Invoke(friendName);
        }
    }
}