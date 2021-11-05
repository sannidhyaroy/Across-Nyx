using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    #region Unity Methods
    private void Start() {
        string randomName = $"Tester{Guid.NewGuid().ToString()}";
        ConnectToPhoton(randomName);
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
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined a Lobby successfully!");
        CreatePhotonRoom("test");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Photon Room '" + PhotonNetwork.CurrentRoom.Name + "' created successfully!");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Photon Room '" + PhotonNetwork.CurrentRoom.Name + "' joined successfully!");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Photon Room Left Successfully!");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("We have encountered an error while trying to join a Photon Room\nError Message: " + message);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.UserId + "has joined this room!");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.UserId + "has left this room!");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log(newMasterClient.UserId + "is now the Room Admin");
    }
    #endregion
}
