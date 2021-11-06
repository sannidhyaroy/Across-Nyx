using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace MainScript
{
    public class DisplayFriendsUI : MonoBehaviour
    {
        [SerializeField] private Transform friendContainer;
        [SerializeField] private FriendsUIList friendsUIListPrefab;
        private void Awake() {
            PhotonFriendsController.OnDisplayFriends += HandleDisplayFriends;
        }
        private void OnDestroy() {
            PhotonFriendsController.OnDisplayFriends -= HandleDisplayFriends;
        }

        private void HandleDisplayFriends(List<FriendInfo> friends)
        {
            foreach (Transform child in friendContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (FriendInfo friend in friends)
            {
                FriendsUIList friendsUIList = Instantiate(friendsUIListPrefab, friendContainer);
                friendsUIList.Initialize(friend);
            }
        }
    }
}
