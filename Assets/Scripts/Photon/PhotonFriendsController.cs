using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PlayFabFriendInfo = PlayFab.ClientModels.FriendInfo;
using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using UnityEngine;

namespace MainScript
{
    public class PhotonFriendsController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float refreshCooldown;
        [SerializeField] private float refreshCountdown;
        [SerializeField] private List<PlayFabFriendInfo> friendList;
        public static Action<List<PhotonFriendInfo>> OnDisplayFriends = delegate { };
        private void Awake()
        {
            PlayFabFriendController.OnFriendListUpdate += HandleFriendsUpdated;
        }

        private void OnDestroy()
        {
            PlayFabFriendController.OnFriendListUpdate -= HandleFriendsUpdated;
        }

        private void Update()
        {
            if (refreshCountdown > 0)
            {
                refreshCountdown -= Time.deltaTime;
            }
            else
            {
                refreshCountdown = refreshCooldown;
                if (PhotonNetwork.InRoom) return;
                FindPhotonFriends(friendList);
            }
        }

        private void HandleFriendsUpdated(List<PlayFabFriendInfo> friends)
        {
            friendList = friends;
            FindPhotonFriends(friendList);
        }

        private void FindPhotonFriends(List<PlayFabFriendInfo> friends)
        {
            Debug.Log($"Handle getting {friends.Count} Photon friends!");
            if (friends.Count != 0)
            {
                string[] friendDisplayNames = friends.Select(f => f.TitleDisplayName).ToArray();
                PhotonNetwork.FindFriends(friendDisplayNames);
            }
            else{
                List<PhotonFriendInfo> friendList = new();
                OnDisplayFriends?.Invoke(friendList);
            }
        }

        public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
        {
            Debug.Log($"Invoke UI to display Photon friends found: {friendList.Count}");
            OnDisplayFriends?.Invoke(friendList);
        }
    }
}