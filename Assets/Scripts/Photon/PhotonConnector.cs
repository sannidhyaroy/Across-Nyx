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
        #region Unity Methods
        public void StartPhotonService()
        {
            InvitesUIList.OnRoomInviteAccept += HandleRoomInviteAccept;
            ConnectToPhoton(PlayerProfile.Username);
        }
        private void OnDestroy()
        {
            InvitesUIList.OnRoomInviteAccept -= HandleRoomInviteAccept;
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

        private void HandleRoomInviteAccept(string roomName)
        {
            PlayerPrefs.SetString("PhotonRoom", roomName);
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                if (PhotonNetwork.InLobby)
                {
                    JoinPlayerRoom();
                }
            }
        }

        private void JoinPlayerRoom()
        {
            string roomName = PlayerPrefs.GetString("PhotonRoom");
            PlayerPrefs.SetString("PhotonRoom", null);
            PhotonNetwork.JoinRoom(roomName);
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
            string roomName = PlayerPrefs.GetString("PhotonRoom");
            if (!string.IsNullOrEmpty(roomName))
            {
                JoinPlayerRoom();
            }
            // else
            // {
            //     CreatePhotonRoom(PhotonNetwork.LocalPlayer.UserId+"'s Room");
            // }
        }
        public override void OnLeftLobby()
        {
            Debug.Log("Photon Lobby Left successfully!");
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Photon Room '" + PhotonNetwork.CurrentRoom.Name + "' created successfully!");
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Photon Room Left Successfully!");
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("We have encountered an error while trying to join a Photon Room\nError Message: " + message);
            FindObjectOfType<PlayFabFriendController>().ErrorMsg.text = "We have encountered an error while trying to join a Photon Room\nError Message: " + message;
            FindObjectOfType<MultiplayerErrorMsg>().ErrorClearer();
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer.UserId + " has joined this room!");
            FindObjectOfType<PlayFabFriendController>().ErrorMsg.text = newPlayer.UserId + " has joined this room!";
            FindObjectOfType<MultiplayerErrorMsg>().ErrorClearer();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log(otherPlayer.UserId + " has left this room!");
            FindObjectOfType<PlayFabFriendController>().ErrorMsg.text = otherPlayer.UserId + " has left this room!";
            FindObjectOfType<MultiplayerErrorMsg>().ErrorClearer();
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log(newMasterClient.UserId + " is now the Room Admin");
            FindObjectOfType<PlayFabFriendController>().ErrorMsg.text = newMasterClient.UserId + " is now the Room Admin";
            FindObjectOfType<MultiplayerErrorMsg>().ErrorClearer();
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
