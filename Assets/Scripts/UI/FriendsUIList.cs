using System;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainScript
{
    public class FriendsUIList : MonoBehaviour
    {
        [SerializeField] private TMP_Text friendNameText;
        [SerializeField] private FriendInfo friend;
        [SerializeField] private Image onlineImage;
        [SerializeField] private Color onlineColor;
        [SerializeField] private Color offlineColor;
        public static Action<string> OnRemoveFriend = delegate { };
        public static Action<string> OnInviteFriend = delegate { };

        public void Initialize(FriendInfo friend)
        {
            this.friend = friend;
            friendNameText.SetText(this.friend.UserId);
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