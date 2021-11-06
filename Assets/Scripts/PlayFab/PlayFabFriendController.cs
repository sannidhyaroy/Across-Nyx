using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

namespace MainScript
{
    public class PlayFabFriendController : MonoBehaviour
    {
        public static Action<List<FriendInfo>> OnFriendListUpdate = delegate { };
        private List<FriendInfo> friends;
        [Header("Assign GameObjects:")]
        [SerializeField] private TextMeshProUGUI ErrorMsg;
        private void Awake()
        {
            friends = new();
            PhotonConnector.GetPhotonFriends += HandleGetFriends;
            AddFriends.OnAddFriend += HandleAddPlayFabFriend;
            FriendsUIList.OnRemoveFriend += HandleRemoveFriend;
        }
        private void OnDestroy()
        {
            PhotonConnector.GetPhotonFriends -= HandleGetFriends;
            AddFriends.OnAddFriend -= HandleAddPlayFabFriend;
            FriendsUIList.OnRemoveFriend -= HandleRemoveFriend;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void HandleGetFriends()
        {
            GetPlayFabFriends();
        }

        private void HandleAddPlayFabFriend(string name)
        {
            var request = new AddFriendRequest
            {
                FriendTitleDisplayName = name,
            };
            PlayFabClientAPI.AddFriend(request, OnFriendAdd, OnError);
        }

        private void HandleRemoveFriend(string name)
        {
            string id = friends.FirstOrDefault(f => f.TitleDisplayName == name).FriendPlayFabId;
            var request = new RemoveFriendRequest
            { FriendPlayFabId = id, };
            PlayFabClientAPI.RemoveFriend(request, OnFriendRemove, OnError);
        }

        private void OnError(PlayFabError error)
        {
            Debug.Log("We have encountered an error while processing your request");
            ErrorMsg.text = error.ErrorMessage;
            Invoke("ClearError", 3);
            Debug.Log("Error Report: " + error.GenerateErrorReport());
        }

        private void GetPlayFabFriends()
        {
            var request = new GetFriendsListRequest
            {
                IncludeFacebookFriends = false,
                IncludeSteamFriends = false,
                XboxToken = null,
            };
            PlayFabClientAPI.GetFriendsList(request, OnFriendListGet, OnError);
        }

        private void OnFriendListGet(GetFriendsListResult result)
        {
            friends = result.Friends;
            OnFriendListUpdate?.Invoke(result.Friends);
        }

        private void OnFriendAdd(AddFriendResult result)
        {
            GetPlayFabFriends();
        }

        private void OnFriendRemove(RemoveFriendResult result)
        {
            GetPlayFabFriends();
        }

        private void ClearError()
        {
            ErrorMsg.text = null;
        }
    }
}
