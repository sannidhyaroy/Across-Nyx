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
        public static Action<List<PhotonFriendInfo>> OnDisplayFriends = delegate { };
        private void Awake()
        {
            PlayFabFriendController.OnFriendListUpdate += HandleFriendsUpdated;
        }

        private void OnDestroy()
        {
            PlayFabFriendController.OnFriendListUpdate -= HandleFriendsUpdated;
        }

        private void HandleFriendsUpdated(List<PlayFabFriendInfo> friends)
        {
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
            OnDisplayFriends?.Invoke(friendList);
        }
    }
}