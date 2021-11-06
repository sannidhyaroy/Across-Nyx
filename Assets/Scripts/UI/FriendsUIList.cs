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
        [SerializeField] private TMP_Text friendNameText;
        [SerializeField] private string friendName;
        [SerializeField] private FriendInfo friend;
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
            // PhotonRoomController.OnRoomStatusChange += HandleInRoom;
        }
        private void OnDestroy()
        {
            PhotonChatController.OnStatusUpdated -= HandleStatusUpdated;
            PhotonChatFriendController.OnStatusUpdated -= HandleStatusUpdated;
            // PhotonRoomController.OnRoomStatusChange -= HandleInRoom;
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(friendName)) return;
            OnGetCurrentStatus?.Invoke(friendName);
            OnGetRoomStatus?.Invoke();
        }

        public void Initialize(FriendInfo friend)
        {
            this.friend = friend;
            friendNameText.SetText(this.friend.UserId);
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
        }

        private void HandleInRoom(bool inRoom)
        {
            Debug.Log($"Updating invite ui to {inRoom}");
            inviteButton.SetActive(inRoom && isOnline);
        }

        private void SetupUI()
        {
            friendNameText.SetText(friendName);
            inviteButton.SetActive(false);
        }
        public void RemoveFriend()
        {
            OnRemoveFriend?.Invoke(friend.UserId);
        }
        public void InviteFriend()
        {
            Debug.Log(friend.UserId + "invited!");
            OnInviteFriend?.Invoke(friend.UserId);
        }
    }
}