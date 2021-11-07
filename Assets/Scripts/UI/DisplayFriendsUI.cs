using System;
using System.Linq;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace MainScript
{
    public class DisplayFriendsUI : MonoBehaviour
    {
        [SerializeField] private Transform friendContainer;
        [SerializeField] private FriendsUIList friendsUIListPrefab;
        private void Awake()
        {
            PhotonFriendsController.OnDisplayFriends += HandleDisplayFriends;
            PhotonChatFriendController.OnDisplayFriends += HandleDisplayChatFriends;
        }
        private void OnDestroy()
        {
            PhotonFriendsController.OnDisplayFriends -= HandleDisplayFriends;
            PhotonChatFriendController.OnDisplayFriends -= HandleDisplayChatFriends;
        }

        private void HandleDisplayFriends(List<FriendInfo> friends)
        {
            foreach (Transform child in friendContainer)
            {
                Destroy(child.gameObject);
            }
            var sortedFriends = friends.OrderByDescending(o => o.IsOnline ? 1 : 0).ThenBy(u => u.UserId);
            foreach (FriendInfo friend in friends)
            {
                FriendsUIList friendsUIList = Instantiate(friendsUIListPrefab, friendContainer);
                friendsUIList.Initialize(friend);
            }
        }
        private void HandleDisplayChatFriends(List<string> friends)
        {
            foreach (Transform child in friendContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (string friend in friends)
            {
                FriendsUIList uifriend = Instantiate(friendsUIListPrefab, friendContainer);
                uifriend.Initialize(friend);
            }
        }
    }
}
