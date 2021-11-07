using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace MainScript
{
    public class PhotonRoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameMode _selectedGameMode;
        [SerializeField] private GameMode[] _availableGameModes;
        [SerializeField] private bool _startGame;
        [SerializeField] private float _currentCountDown;
        [SerializeField] private int _gameSceneIndex;

        private const string GAME_MODE = "GAMEMODE";
        private const string START_GAME = "STARTGAME";
        private const float GAME_COUNT_DOWN = 10f;

        public static Action<GameMode> OnJoinRoom = delegate { };
        public static Action<bool> OnRoomStatusChange = delegate { };
        public static Action OnRoomLeft = delegate { };
        public static Action<Player> OnOtherPlayerLeftRoom = delegate { };
        public static Action<Player> OnMasterOfRoom = delegate { };
        public static Action<float> OnCountingDown = delegate { };

        private void Awake()
        {
            GameModeUI.OnGameModeSelected += HandleGameModeSelected;
            InvitesUIList.OnRoomInviteAccept += HandleRoomInviteAccept;
            PhotonConnector.OnLobbyJoined += HandleLobbyJoined;
            DisplayRoomUI.OnLeaveRoom += HandleLeaveRoom;
            DisplayRoomUI.OnStartGame += HandleStartGame;
            FriendsUIList.OnGetRoomStatus += HandleGetRoomStatus;
            PlayerSelectionUI.OnKickPlayer += HandleKickPlayer;

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void OnDestroy()
        {
            GameModeUI.OnGameModeSelected -= HandleGameModeSelected;
            InvitesUIList.OnRoomInviteAccept -= HandleRoomInviteAccept;
            PhotonConnector.OnLobbyJoined -= HandleLobbyJoined;
            DisplayRoomUI.OnLeaveRoom -= HandleLeaveRoom;
            DisplayRoomUI.OnStartGame -= HandleStartGame;
            FriendsUIList.OnGetRoomStatus -= HandleGetRoomStatus;
            PlayerSelectionUI.OnKickPlayer -= HandleKickPlayer;
        }

        private void Update()
        {
            if (!_startGame) return;

            if (_currentCountDown > 0)
            {
                OnCountingDown?.Invoke(_currentCountDown);
                _currentCountDown -= Time.deltaTime;
            }
            else
            {
                _startGame = false;

                Debug.Log("Loading level!");
                PhotonNetwork.LoadLevel(_gameSceneIndex);
            }
        }

        #region Handle Methods
        private void HandleGameModeSelected(GameMode gameMode)
        {
            if (!PhotonNetwork.IsConnectedAndReady) return;
            if (PhotonNetwork.InRoom) return;

            _selectedGameMode = gameMode;
            Debug.Log($"Joining new {_selectedGameMode.Name} game");
            JoinPhotonRoom();
        }

        private void HandleRoomInviteAccept(string roomName)
        {
            PlayerPrefs.SetString("PhotonRoom", roomName);
            if (PhotonNetwork.InRoom)
            {
                OnRoomLeft?.Invoke();
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.JoinRoom(roomName);
                    PlayerPrefs.SetString("PhotonRoom", "");
                }
            }
        }

        private void HandleLobbyJoined()
        {
            // string roomName = PlayerProfile.Username;
            // if (!string.IsNullOrEmpty(roomName))
            // {
            //     PhotonNetwork.JoinRoom(roomName);
            //     PlayerPrefs.SetString("PhotonRoom", "");
            // }
        }

        private void HandleLeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                OnRoomLeft?.Invoke();
                PhotonNetwork.LeaveRoom();
            }
        }

        private void HandleGetRoomStatus()
        {
            OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
        }

        private void HandleStartGame()
        {
            Hashtable startRoomProperty = new Hashtable()
            { {START_GAME, true} };
            PhotonNetwork.CurrentRoom.SetCustomProperties(startRoomProperty);
        }

        private void HandleKickPlayer(Player kickedPlayer)
        {
            if (PhotonNetwork.LocalPlayer.Equals(kickedPlayer))
            {
                HandleLeaveRoom();
            }
        }
        #endregion

        #region Private Methods
        private void JoinPhotonRoom()
        {
            Hashtable expectedCustomRoomProperties = new Hashtable()
            { {GAME_MODE, _selectedGameMode.Name} };

            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        }

        private void CreatePhotonRoom()
        {
            string roomName = Guid.NewGuid().ToString();
            RoomOptions ro = GetRoomOptions();

            PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
        }

        private RoomOptions GetRoomOptions()
        {
            RoomOptions ro = new RoomOptions();
            ro.IsOpen = true;
            ro.IsVisible = true;
            ro.MaxPlayers = _selectedGameMode.MaxPlayers;
            ro.PublishUserId = true;

            string[] roomProperties = { GAME_MODE };

            Hashtable customRoomProperties = new Hashtable()
            { {GAME_MODE, _selectedGameMode.Name} };

            ro.CustomRoomPropertiesForLobby = roomProperties;
            ro.CustomRoomProperties = customRoomProperties;

            return ro;
        }

        private void DebugPlayerList()
        {
            string players = "";
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                players += $"{player.Value.NickName}, ";
            }
            Debug.Log($"Current Room Players: {players}");
        }

        private GameMode GetRoomGameMode()
        {
            string gameModeName = (string)PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE];
            GameMode gameMode = null;
            for (int i = 0; i < _availableGameModes.Length; i++)
            {
                if (string.Compare(_availableGameModes[i].Name, gameModeName) == 0)
                {
                    gameMode = _availableGameModes[i];
                    break;
                }
            }
            return gameMode;
        }

        private void AutoStartGame()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= _selectedGameMode.MaxPlayers)
                HandleStartGame();
        }
        #endregion

        #region Photon Callbacks
        public override void OnCreatedRoom()
        {
            Debug.Log("Photon Room '" + PhotonNetwork.CurrentRoom.Name + "' created successfully!");
            OnMasterOfRoom?.Invoke(PhotonNetwork.LocalPlayer);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Photon Room '" + PhotonNetwork.CurrentRoom.Name + "' joined successfully!");
            FindObjectOfType<MultiplayerMenu>().OnJoinedRoom();
            DebugPlayerList();

            _selectedGameMode = GetRoomGameMode();
            OnJoinRoom?.Invoke(_selectedGameMode);
            OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Photon Room Left Successfully!");
            FindObjectOfType<MultiplayerMenu>().OnLeftRoom();
            _selectedGameMode = null;
            _startGame = false;
            OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"OnJoinRandomFailed {message}");
            CreatePhotonRoom();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("We have encountered an error while trying to join a Photon Room\nError Message: " + message);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer.UserId + " has joined this room!");
            DebugPlayerList();
            AutoStartGame();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log(otherPlayer.UserId + " has left this room!");
            OnOtherPlayerLeftRoom?.Invoke(otherPlayer);
            DebugPlayerList();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log(newMasterClient.UserId + " is now the Room Admin");
            OnMasterOfRoom?.Invoke(newMasterClient);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startGameObject;
            if (propertiesThatChanged.TryGetValue(START_GAME, out startGameObject))
            {
                _startGame = (bool)startGameObject;
                if (_startGame)
                {
                    _currentCountDown = GAME_COUNT_DOWN;
                }
                if (_startGame && PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
        }
        #endregion
    }
}