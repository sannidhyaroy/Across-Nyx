using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MainScript
{
    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        public static Action GetPhotonFriends = delegate { };
        public static Action OnLobbyJoined = delegate { };
        #region Unity Methods
        public void StartPhotonService()
        {
            if (PhotonNetwork.IsConnectedAndReady || PhotonNetwork.IsConnected) return;

            ConnectToPhoton(PlayerProfile.Username);
        }
        #endregion
        #region Private Methods
        private void ConnectToPhoton(string UserID)
        {
            Debug.Log("Connecting to Photon as " + UserID);
            PhotonNetwork.AuthValues = new AuthenticationValues(UserID);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = UserID;
            PhotonNetwork.ConnectUsingSettings();
        }
        private void CreatePhotonRoom(string RoomName)
        {
            RoomOptions ro = new RoomOptions();
            ro.IsOpen = true;
            ro.IsVisible = true;
            ro.MaxPlayers = 4;
            ro.PublishUserId = true;
            PhotonNetwork.JoinOrCreateRoom(RoomName, ro, TypedLobby.Default);
        }
        #endregion
        #region Public Methods
        #endregion
        #region Photon Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master Server");
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected from Photon Servers\nReason: "+cause);
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined a Lobby successfully!");
            Debug.Log("Retrieving Friends List from PlayFab");
            GetPhotonFriends?.Invoke();
            OnLobbyJoined?.Invoke();
        }
        public override void OnLeftLobby()
        {
            Debug.Log("Photon Lobby Left successfully!");
        }
        #endregion
        #region Button Click Events
        public void OnClick_CreateRoom(string roomName)
        {
            CreatePhotonRoom(roomName);
        }
        public void OnClick_JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        public void OnClick_LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion
    }
}
